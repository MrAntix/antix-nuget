using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Antix.NuGet.API.Packages;
using Antix.NuGet.API.Packages.Filters;
using Castle.MicroKernel.Registration;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Testing;
using Moq;
using Xunit;

namespace Antix.NuGet.Tests.Packages
{
    public class PushPackageTest
    {
        const string PackagePath = "resources\\Antix.code.nupkg";

        [Fact]
        public void integration_happiness()
        {
            const string baseAddress = "http://localhost:9100";

            using (WebApp.Start<TestStartup>(baseAddress))
            {
                var parameters = string.Format(
                    "push {0} -ApiKey {1} -Source {2}/packages",
                    PackagePath,
                    PackagesSettings.Default.APIKey,
                    baseAddress);

                Console.WriteLine(parameters);

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo(
                        @"..\..\..\.NuGet\nuget.exe",
                        parameters
                        )
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                process.Start();

                Console.WriteLine(process.StandardOutput.ReadToEnd());
                Console.WriteLine(process.StandardError.ReadToEnd());

                process.WaitForExit();
                Thread.Sleep(150);

                Assert.Equal(0, process.ExitCode);
            }
        }

        [Fact]
        public void when_no_key_expected()
        {
            using (var server = TestServer.Create(appBuilder =>
            {
                var startup = new TestStartup();
                startup.Configuration(appBuilder);

                startup.Container.Register(
                    Component.For<IPackagesSettings>()
                        .Instance(GetSettings())
                        .IsDefault()
                    );
            }))
            {
                var request = GetRequest(Guid.Empty);

                var response =
                    server.HttpClient.SendAsync(request).Result;

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact]
        public void when_key_expected_but_is_not_supplied()
        {
            using (var server = TestServer.Create<TestStartup>())
            {
                var request = GetRequest(Guid.Empty);

                var response =
                    server.HttpClient.SendAsync(request).Result;

                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }
        }

        [Fact]
        public void when_key_expected_but_is_incorrect()
        {
            using (var server = TestServer.Create<TestStartup>())
            {
                var request = GetRequest(Guid.NewGuid());

                var response =
                    server.HttpClient.SendAsync(request).Result;

                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }
        }

        [Fact]
        public void when_key_expected_and_correct()
        {
            IPackagesSettings settings = null;

            using (var server = TestServer.Create(appBuilder =>
            {
                var startup = new TestStartup();
                startup.Configuration(appBuilder);

                settings = startup.Container.Resolve<IPackagesSettings>();
            }))
            {
                var request = GetRequest(settings.APIKey);

                var response =
                    server.HttpClient.SendAsync(request).Result;

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        static IPackagesSettings GetSettings(Guid apiKey = default (Guid))
        {
            var mock = new Mock<IPackagesSettings>();
            mock.Setup(o => o.APIKey).Returns(apiKey);
            mock.Setup(o => o.PackageRoot).Returns(PackagesSettings.Default.PackageRoot);

            return mock.Object;
        }

        static HttpRequestMessage GetRequest(Guid apiKey)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Put,
                "/packages");

            if (!Guid.Empty.Equals(apiKey))
                request.Headers.Add(
                    APIKeyFilter.HEADER_NAME,
                    apiKey.ToString());

            var nupkg = new ByteArrayContent(File.ReadAllBytes(PackagePath));
            nupkg.Headers.ContentDisposition
                = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = "package"
                };
            nupkg.Headers.ContentDisposition.Parameters
                .Add(new NameValueHeaderValue("name", "package"));
            nupkg.Headers.ContentType
                = new MediaTypeHeaderValue("application/octet-stream");

            request.Content = new MultipartContent
            {
                nupkg
            };

            return request;
        }
    }
}