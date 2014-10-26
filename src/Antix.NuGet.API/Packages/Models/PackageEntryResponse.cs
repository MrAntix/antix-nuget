using Antix.NuGet.Packages.Models;

namespace Antix.NuGet.API.Packages.Models
{
    public class PackageEntryResponse
    {
        public string RequestAuthorityUri { get; set; }
        public IPackageMetadata Package { get; set; }
    }
}