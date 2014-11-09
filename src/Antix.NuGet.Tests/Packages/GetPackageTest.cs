using System;
using System.Net;
using System.Threading;
using Microsoft.Owin.Hosting;
using Xunit;

namespace Antix.NuGet.Tests.Packages
{
    public class GetPackageTest
    {
        [Fact]
        public void integration_happiness()
        {
            const string baseAddress = "http://localhost:9100";
            var baseUri = new Uri(baseAddress);

            using (WebApp.Start<TestStartup>(baseAddress))

            using (var webClient = new WebClient())
            {
                var countUri = new Uri(baseUri, "/feed/Packages()/$count");
                var count = webClient.DownloadString(countUri);
                while (count == "0")
                {
                    Thread.Sleep(100);
                    count = webClient.DownloadString(countUri);
                }

                var requestUri = new Uri(
                    new Uri(baseAddress),
                    "/package/Antix.code/6.0.0.9-beta");

                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.AllowAutoRedirect = false;

                var response = request.GetResponse();

                Assert.Equal("http://localhost:9100/content/packages/Antix/code/Antix.code.6.0.0.9-beta.nupkg", response.Headers["location"]);
            }
        }
    }
}