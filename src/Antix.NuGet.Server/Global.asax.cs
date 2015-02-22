using System;
using System.Threading;
using System.Web;
using Antix.Logging;
using Antix.NuGet.Server.Configuration;

namespace Antix.NuGet.Server
{
    public class Global : HttpApplication
    {
        protected void Application_Error(Object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            if (ex is ThreadAbortException)
                return;

            WindsorConfig.LogDeletate
                .Error(m => m("Unhandled Exception"), ex);
        }
    }
}