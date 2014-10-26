using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Antix.NuGet.Application.Packages.Models;
using Antix.NuGet.Application.Packages.Storage;
using Antix.NuGet.Packages;
using Antix.NuGet.Packages.Models;
using Antix.Services.Models;

namespace Antix.NuGet.Application.Packages
{
    public class FileSystemGetPackageService :
        IGetPackageService
    {
        readonly IFileSystemPackageMetadataStore _store;

        public FileSystemGetPackageService(
            IFileSystemPackageMetadataStore store)
        {
            _store = store;
        }

        public async Task<Response<Package>> ExecuteAsync(GetPackageRequest model)
        {
            var query = _store.Items
                .ToArray()
                .Cast<FileSystemPackageMetadata>()
                .Where(i => i.Id.Equals(model.Id, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(model.Version))
                query = query.Where(i => i.Version.Equals(model.Version, StringComparison.OrdinalIgnoreCase));

            var metaData = query.First();

            return Response.Data(
                new Package(metaData, File.OpenRead(metaData.Path)));
        }
    }
}