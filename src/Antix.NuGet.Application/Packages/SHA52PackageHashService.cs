using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Antix.NuGet.Packages;

namespace Antix.NuGet.Application.Packages
{
    public class SHA52PackageHashService :
        IPackageHashService
    {
        readonly SHA512 _hashService;

        public SHA52PackageHashService()
        {
            _hashService = SHA512.Create();
        }

        public async Task<string> ExecuteAsync(string path)
        {
            using (var file = File.OpenRead(path))
            {
                var hash = _hashService.ComputeHash(file);
                return Convert.ToBase64String(hash);
            }
        }

        public string Algorithm
        {
            get { return "SHA512"; }
        }
    }
}