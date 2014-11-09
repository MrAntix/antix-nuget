using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Antix.NuGet.Server.Configuration;
using Microsoft.Owin.Hosting;
using Xunit;

namespace Antix.NuGet.Tests.Packages
{
    public class GetPackageTest
    {
        [Fact(Skip="not working")]
        public void integration_happiness()
        {
            const string baseAddress = "http://localhost:9100";
            var baseUri = new Uri(baseAddress);

            using (WebApp.Start<Startup>(baseAddress))
            using(var webClient = new WebClient())
            {
                var countUri = new Uri(baseUri, "/feed/Packages()/$count");
                var count = webClient.DownloadString(countUri);
                while (count == "0")
                {
                    Thread.Sleep(500);
                    count = webClient.DownloadString(countUri);
                }

                var response = webClient.DownloadData(
                    new Uri(
                        new Uri(baseAddress),
                        "/package/antix.code/6.0.0.9-beta"));

                Assert.NotNull(response);
            }
        }
    }
}