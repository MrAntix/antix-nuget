using System;
using System.Threading;
using System.Web;
using System.Web.Http;
using Antix.Logging;
using Antix.NuGet.Application.Packages.Storage;
using Antix.NuGet.Server.Configuration;
using Castle.Windsor;

namespace Antix.NuGet.Server
{
    public class Global : HttpApplication
    {
        IFileSystemPackageMetadataStore _store;

        protected void Application_Start(object sender, EventArgs e)
        {
            var container = new WindsorContainer()
                .Configure(GlobalConfiguration.Configuration);

            // TODO temporary, store will be called by services which require it
            // eg the get/search services

            _store = container.Resolve<IFileSystemPackageMetadataStore>();
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            if (ex is ThreadAbortException)
                return;

            var log = (Log.Delegate) GlobalConfiguration
                .Configuration
                .DependencyResolver
                .GetService(typeof (Log.Delegate));
            log.Error(m => m(ex, "Unhandled Exception"));
        }
    }
}