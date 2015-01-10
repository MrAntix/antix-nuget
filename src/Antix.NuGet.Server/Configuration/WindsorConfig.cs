using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;
using Antix.Http.Dispatcher;
using Antix.Http.Filters;
using Antix.Http.Filters.Logging;
using Antix.IO;
using Antix.Logging;
using Antix.NuGet.API.Packages;
using Antix.NuGet.API.Packages.Filters;
using Antix.NuGet.API.Packages.Formatters;
using Antix.NuGet.Application.Events;
using Antix.NuGet.Application.Hubs;
using Antix.NuGet.Application.Packages.Models;
using Antix.NuGet.Application.Packages.Storage;
using Antix.NuGet.Events;
using Antix.NuGet.Packages.Models;
using Antix.Services;
using Antix.SignalR;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Antix.NuGet.Server.Configuration
{
    public static class WindsorConfig
    {
        public static IWindsorContainer Configure(
            this IWindsorContainer container,
            HttpConfiguration httpConfiguration,
            HubConfiguration hubConfiguration
            )
        {
            container.AddFacility(new TypedFactoryFacility());

            RegisterLogging(container);
            RegisterServices(container);

            RegisterWebApi(container, httpConfiguration);
            RegisterSignarR(container, hubConfiguration);

            return container;
        }

        static void RegisterSignarR(
            IWindsorContainer container,
            ConnectionConfiguration hubConfiguration)
        {
            // cannot use, signalr has special requirements eg JSON ContractResolver below
            //hubConfiguration.Resolver
            //    = new WindsorDependencyResolver(container, LogDeletate);

            hubConfiguration.Resolver
                .Register(
                    typeof (IHubActivator),
                    () => new SignalRHubActivator(container.Resolve));
            hubConfiguration.Resolver
                .Register(
                    typeof (JsonSerializer),
                    () => new JsonSerializer
                    {
                        ContractResolver = new SignalRContractResolver()
                    });

            container.Register(
                Classes
                    .FromAssemblyContaining<EventsHub>()
                    .BasedOn<IHub>()
                    .WithServiceSelf()
                    .LifestyleTransient()
                );

            container.Register(
                Component.For<IEventsBus>()
                    .UsingFactoryMethod(k => new EventsBus(
                        hubConfiguration.Resolver.Resolve<IConnectionManager>()
                            .GetHubContext<EventsHub>()
                        ))
                    .LifestyleTransient()
                );

            container.Register(
                Component.For<IJavaScriptMinifier>()
                    .ImplementedBy<JavaScriptMinifier>()
                    .LifestyleSingleton()
                );
        }

        static void RegisterLogging(IWindsorContainer container)
        {
            container.Register(
                Component.For<Log.Delegate>()
                    .Instance(LogDeletate)
                    .LifestyleSingleton()
                );
        }

        static void RegisterServices(
            IWindsorContainer container)
        {
            container.Register(
                Component.For<JsonSerializer>()
                    .UsingFactoryMethod(k => new JsonSerializer
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
                );
            container.Register(
                Component.For<IFileSystemChangeMonitor>()
                    .ImplementedBy<FileSystemChangeMonitor>()
                    .LifestyleTransient()
                );
            container.Register(
                Component.For<IFileSystemPackageMetadataStore>()
                    .ImplementedBy<FileSystemPackageMetadataStore>()
                    .LifestyleSingleton()
                );

            container.Register(
                Classes
                    .FromAssemblyContaining<Startup>()
                    .BasedOn<IService>()
                    .WithServiceAllInterfaces()
                    .WithServiceSelf()
                    .LifestyleTransient()
                );
            container.Register(
                Classes
                    .FromAssemblyContaining<Package>()
                    .BasedOn<IService>()
                    .WithServiceAllInterfaces()
                    .WithServiceSelf()
                    .LifestyleTransient()
                );
            container.Register(
                Classes
                    .FromAssemblyContaining<FileSystemPackageMetadata>()
                    .BasedOn<IService>()
                    .WithServiceAllInterfaces()
                    .WithServiceSelf()
                    .LifestyleTransient()
                );
            container.Register(
                Classes
                    .FromAssemblyContaining<PackagesSettings>()
                    .BasedOn<IService>()
                    .WithServiceAllInterfaces()
                    .WithServiceSelf()
                    .LifestyleTransient()
                );
        }

        static void RegisterWebApi(
            IWindsorContainer container,
            HttpConfiguration configuration)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<ApiController>()
                    .LifestyleTransient()
                );
            container.Register(
                Classes.FromAssemblyContaining<PackagesFeedController>()
                    .BasedOn<ApiController>()
                    .LifestyleTransient()
                );

            configuration.Services.Replace(
                typeof (IHttpControllerActivator),
                new ServiceHttpControllerActivator(
                    t => (IHttpController) container.Resolve(t),
                    container.Release,
                    container.Resolve<Log.Delegate>())
                );

            configuration.Services.Replace(
                typeof (IFilterProvider),
                new ServiceFilterProvider(container.Resolve)
                );

            container.Register(
                Classes
                    .FromAssemblyContaining<APIKeyFilter>()
                    .BasedOn<IFilter>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
                    .LifestyleTransient()
                );

            container.Register(
                Classes
                    .FromAssemblyContaining<LogActionFilter>()
                    .BasedOn<IFilter>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
                    .LifestyleTransient()
                );

            configuration.MapHttpAttributeRoutes();

            var formatter = configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver
                = new CamelCasePropertyNamesContractResolver();

            configuration.Formatters.Clear();
            configuration.Formatters.Add(formatter);
            configuration.Formatters.Add(new PackageFeedFormatter());
            configuration.Formatters.Add(new PackageEntryFormatter());

            //configuration.Filters.Add(new ResponseGlobalFilter());

            configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            configuration.EnsureInitialized();
        }

        static Log.Delegate _log;

        public static Log.Delegate LogDeletate
        {
            get { return _log ?? (_log = CreateLogDelegate()); }
        }

        static Log.Delegate CreateLogDelegate()
        {
            return Log.DEBUG
                ? Log.ToDebug
                : Log.ToTrace;
        }
    }
}