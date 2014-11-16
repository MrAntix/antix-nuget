using System.Web.Http;
using System.Web.Optimization;
using Antix.NuGet.Server.Configuration;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof (Startup))]
namespace Antix.NuGet.Server.Configuration
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var container = new WindsorContainer()
                .Configure(
                    GlobalConfiguration.Configuration,
                    new HubConfiguration());

            app.MapSignalR();
        }
    }
}