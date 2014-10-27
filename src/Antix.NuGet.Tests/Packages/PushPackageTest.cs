using System;
using System.Diagnostics;
using System.Threading;
using Antix.NuGet.Server.Configuration;
using Microsoft.Owin.Hosting;
using Xunit;

namespace Antix.NuGet.Tests.Packages
{
    public class PushPackageTest
    {
        [Fact]
        public void integration_happiness()
        {
            const string baseAddress = "http://localhost:9100";

            using (WebApp.Start<Startup>(baseAddress))
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo(
                        @"..\..\..\.NuGet\nuget.exe",
                        string.Format(
                            "push {0} -ApiKey {1} -Source {2}/packages",
                            "resources\\Antix.code.nupkg",
                            "API-KEY",
                            baseAddress)
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
    }
}