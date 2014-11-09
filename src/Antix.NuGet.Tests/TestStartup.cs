using System.Web.Http;
using Antix.NuGet.Server.Configuration;
using Castle.Windsor;
using Owin;

namespace Antix.NuGet.Tests
{
    public class TestStartup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var apiConfig =
                new HttpConfiguration();

            new WindsorContainer()
                .Configure(apiConfig);

            appBuilder.UseWebApi(apiConfig);
        }
    }
}