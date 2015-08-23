using Antix.NuGet.Packages.Models;

namespace Antix.NuGet.API.Packages.Models
{
    public class PackageFeedResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string RequestAuthorityUri { get; set; }
        public IPackageMetadata[] Packages { get; set; }
    }
}