using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel.Syndication;
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
using Antix.NuGet.API.Packages.Formatters;
using Antix.NuGet.Application.Packages.Models;
using Antix.NuGet.Application.Packages.Storage;
using Antix.NuGet.Packages.Models;
using Antix.Services;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Newtonsoft.Json.Serialization;

namespace Antix.NuGet.Server.Configuration
{
    public static class WindsorConfig
    {
        public static IWindsorContainer Configure(
            this IWindsorContainer container,
            HttpConfiguration configuration)
        {
            container.AddFacility(new TypedFactoryFacility());

            RegisterLogging(container);
            RegisterServices(container);

            RegisterWebApi(container, configuration);

            return container;
        }

        static void RegisterLogging(IWindsorContainer container)
        {
            if (Log.DEBUG)
            {
                container.Register(
                    Component.For<Log.Delegate>()
                        .Instance(Log.ToDebug)
                        .LifestyleSingleton()
                    );
            }
            else
            {
                container.Register(
                    Component.For<Log.Delegate>()
                        .Instance(l => (ex, f, a) =>
                        {
                            var m = string.Format(f, a);
                            Trace.WriteLine(string.Format(
                                "{0:mm:ss:ffff} [{1}]: {2}", DateTime.UtcNow, l, m));
                            if (ex != null)
                            {
                                Trace.WriteLine(ex);
                            }

                            Trace.Flush();
                            Trace.Close();
                        })
                        .LifestyleSingleton()
                    );
            }
        }

        static void RegisterServices(
            IWindsorContainer container)
        {
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
                    .FromAssemblyContaining<Global>()
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
    }
}