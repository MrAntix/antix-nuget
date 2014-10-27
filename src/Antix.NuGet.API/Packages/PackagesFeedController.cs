using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Antix.NuGet.API.Packages.Models;
using Antix.NuGet.Application.Packages.Storage;

namespace Antix.NuGet.API.Packages
{
    [RoutePrefix("feed")]
    public class PackagesFeedController :
        ApiController
    {
        readonly IFileSystemPackageMetadataStore _store;

        public PackagesFeedController(
            IFileSystemPackageMetadataStore store)
        {
            _store = store;
        }

        [Route("")]
        public async Task<HttpResponseMessage> GetRoot()
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(XmlResources.Root, Encoding.UTF8, "application/xml")
            };
        }

        [Route("$metadata")]
        public async Task<HttpResponseMessage> GetMetadata()
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(XmlResources.Metadata, Encoding.UTF8, "application/xml")
            };
        }

        [Route("Packages()/$count")]
        public int GetFeedCount()
        {
            return _store.Items.Count();
        }

        [Route("Packages")]
        [Route("Packages()")]
        public PackageFeedResponse GetFeed()
        {
            var query =
                (from i in _store.Items.ToArray()
                    .OrderByDescending(i => i.CreatedOn)
                    group i by i.Id
                    into g
                    select g.First())
                    .OrderBy(i => i.Id);

            return new PackageFeedResponse
            {
                RequestAuthorityUri = Request.RequestUri.GetLeftPart(UriPartial.Authority),
                Packages = query.ToArray()
            };
        }

        [Route("FindPackagesById()")]
        public PackageEntryResponse GetEntry(string id)
        {
            id = id.Trim('\'');

            var query = _store.Items
                .Where(i =>
                    i.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(i => i.CreatedOn);

            return new PackageEntryResponse
            {
                RequestAuthorityUri = Request.RequestUri.GetLeftPart(UriPartial.Authority),
                Package = query.FirstOrDefault()
            };
        }

        [Route("Packages(Id='{id}',Version='{version}')")]
        public PackageEntryResponse GetEntry(string id, string version)
        {
            var query = _store.Items
                .Where(i =>
                    i.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(version))
            {
                query = query
                    .Where(i => i.Version.Equals(version, StringComparison.OrdinalIgnoreCase));
            }

            return new PackageEntryResponse
            {
                RequestAuthorityUri = Request.RequestUri.GetLeftPart(UriPartial.Authority),
                Package = query.Single()
            };
        }
    }
}