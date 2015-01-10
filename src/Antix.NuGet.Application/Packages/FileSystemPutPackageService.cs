using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Antix.NuGet.Application.Packages.Storage;
using Antix.NuGet.Packages;
using Antix.NuGet.Packages.Models;
using Antix.Services.Models;

namespace Antix.NuGet.Application.Packages
{
    public class FileSystemPutPackageService :
        IPutPackageService
    {
        readonly IFileSystemStorageSettings _options;
        readonly IPackageInfoService _packageInfoService;

        public FileSystemPutPackageService(
            IFileSystemStorageSettings options,
            IPackageInfoService packageInfoService)
        {
            _options = options;
            _packageInfoService = packageInfoService;
        }

        public async Task<IServiceResponse> ExecuteAsync(
            PutPackageRequest model)
        {
            var metadata = await _packageInfoService.ExecuteAsync(model.Path);
            var storePath = CreateStoreStructure(metadata, _options.GetAbsoluteRootDirectory());

            using (var source = File.Open(model.Path, FileMode.Open))
            using (var destination = File.Create(storePath))
            {
                await source.CopyToAsync(destination);
            }

            return ServiceResponse.Empty;
        }

        static string CreateStoreStructure(PackageInfo package, string rootDirectory)
        {
            var name = (string) package.NuSpec.metadata.id;
            var version = package.NuSpec.metadata.version;

            var storePath = name.Split('.')
                .Aggregate(rootDirectory, Path.Combine);
            if (!Directory.Exists(storePath)) Directory.CreateDirectory(storePath);

            return
                Path.Combine(storePath,
                    string.Format("{0}.{1}{2}", name, version, Package.EXTN));
        }
    }
}