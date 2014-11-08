using System;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using Antix.Logging;
using Antix.NuGet.Server.Configuration;
using Castle.Windsor;

namespace Antix.NuGet.Server
{
    public class Global : HttpApplication
    {
        Log.Delegate _log;

        protected void Application_Start(object sender, EventArgs e)
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var container = new WindsorContainer()
                .Configure(GlobalConfiguration.Configuration);

            _log = container.Kernel.Resolve<Log.Delegate>();
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            if (ex is ThreadAbortException)
                return;

            _log.Error(m => m(ex, "Unhandled Exception"));
        }
    }
}