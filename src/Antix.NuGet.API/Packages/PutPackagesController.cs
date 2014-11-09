using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Antix.NuGet.API.Packages.Filters;
using Antix.NuGet.Packages;
using Antix.NuGet.Packages.Models;

namespace Antix.NuGet.API.Packages
{
    public class PutPackagesController :
        ApiController
    {
        readonly IPutPackageService _putPackageService;

        public PutPackagesController(
            IPutPackageService putPackageService)
        {
            _putPackageService = putPackageService;
        }

        [APIKey]
        [Route("packages")]
        public async Task Put()
        {
            var streamProvider
                = new MultipartFormDataStreamProvider(
                    Path.GetTempPath()
                    );
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            var fileData = streamProvider.FileData.Single();
            try
            {
                await _putPackageService.ExecuteAsync(new PutPackageRequest
                {
                    Path = fileData.LocalFileName
                });
            }
            finally
            {
                File.Delete(fileData.LocalFileName);
            }
        }
    }
}