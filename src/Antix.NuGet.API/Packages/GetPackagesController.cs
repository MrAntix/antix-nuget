using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Antix.NuGet.Application.Packages.Models;
using Antix.NuGet.Application.Packages.Storage;

namespace Antix.NuGet.API.Packages
{
    public class GetPackagesController :
        ApiController
    {
        readonly IFileSystemPackageMetadataStore _store;

        public GetPackagesController(IFileSystemPackageMetadataStore store)
        {
            _store = store;
        }

        [Route("package/{id}/{version}")]
        public async Task<RedirectResult> GetPackage(string id, string version)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (version == null) throw new ArgumentNullException("version");

            var package = (FileSystemPackageMetadata) _store.Items.First(
                i => i.Id.Equals(id, StringComparison.OrdinalIgnoreCase)
                     && i.Version.Equals(version, StringComparison.OrdinalIgnoreCase));

            var redirectUri = new Uri(
                new Uri(Request.RequestUri.GetLeftPart(UriPartial.Authority)),
                package.Path.Substring(
                    package.Path.IndexOf("\\content", StringComparison.OrdinalIgnoreCase))
                );

            return Redirect(redirectUri);
        }
    }
}