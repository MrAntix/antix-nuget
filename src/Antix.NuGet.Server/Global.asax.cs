using System;
using System.Threading;
using System.Web;
using System.Web.Http;
using Antix.Logging;
using Antix.NuGet.Server.Configuration;
using Castle.Windsor;

namespace Antix.NuGet.Server
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new WindsorContainer()
                .Configure(GlobalConfiguration.Configuration);
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