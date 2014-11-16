using System.Web.Http;
using Antix.NuGet.Server.Configuration;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;
using Owin;

namespace Antix.NuGet.Tests
{
    public class TestStartup
    {
        public IWindsorContainer Container { get; set; }

        public void Configuration(IAppBuilder appBuilder)
        {
            var apiConfig = new HttpConfiguration();
            var hubConfig = new HubConfiguration();

            Container = new WindsorContainer()
                .Configure(apiConfig, hubConfig);

            appBuilder.UseWebApi(apiConfig);
        }
    }
}