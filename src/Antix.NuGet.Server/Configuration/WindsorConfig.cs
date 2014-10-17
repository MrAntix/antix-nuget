using System;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;
using Antix.Http.Dispatcher;
using Antix.Http.Filters;
using Antix.Http.Filters.Logging;
using Antix.Http.Filters.Response.Antix.Http.Filters.Response;
using Antix.Logging;
using Antix.NuGet.API.Packages;
using Antix.NuGet.Application.Packages;
using Antix.Services;
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
                        .Instance(Log.ToConsole)
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
                Classes
                    .FromAssemblyContaining<Global>()
                    .BasedOn<IService>()
                    .WithServiceAllInterfaces()
                    .WithServiceSelf()
                    .LifestyleTransient()
                );
            container.Register(
                Classes
                    .FromAssemblyContaining<FileSystemPutPackageService>()
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
                Classes.FromAssemblyContaining<PackagesController>()
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

            configuration.EnsureInitialized();

            configuration.Filters.Add(new ResponseGlobalFilter());
        }
    }
}