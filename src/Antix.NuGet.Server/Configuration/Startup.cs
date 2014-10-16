using System.Web.Http;
using Castle.Windsor;
using Owin;

namespace Antix.NuGet.Server.Configuration
{
    public class Startup
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