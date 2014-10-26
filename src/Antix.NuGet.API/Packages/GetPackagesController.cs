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
            var package = (FileSystemPackageMetadata) _store.Items.First(
                i => i.Id.Equals(id, StringComparison.OrdinalIgnoreCase)
                     && i.Version.Equals(version, StringComparison.OrdinalIgnoreCase));


            return Redirect(new Uri(
                new Uri(Request.RequestUri.GetLeftPart(UriPartial.Authority)),
                package.Path.Substring(
                    package.Path.IndexOf("\\content", StringComparison.OrdinalIgnoreCase))
                ));
        }

        //public async Task<HttpResponseMessage> GetPackage(string id, string version)
        //{
        //    var result = await _getPackageService
        //        .ExecuteAsync(new GetPackageRequest
        //        {
        //            Id = id,
        //            Version = version
        //        });

        //    var response = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new StreamContent(result.Data.Stream)
        //    };

        //    response.Content.Headers
        //        .ContentType = new MediaTypeHeaderValue("application/zip");
        //    response.Content.Headers.Add("Content-MD5", result.Data.MD5);
        //    response.Content.Headers.LastModified = result.Data.Created;
        //    response.Headers.ETag = new EntityTagHeaderValue(string.Format("\"{0}-{1}\"", id, version));
        //    response.Headers.AcceptRanges.Add("bytes");

        //    return response;
        //}
    }
}