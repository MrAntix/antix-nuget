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
        readonly IPackageReader _packageReader;

        public FileSystemPutPackageService(
            IFileSystemStorageSettings options,
            IPackageReader packageReader)
        {
            _options = options;
            _packageReader = packageReader;
        }

        public async Task<Response> ExecuteAsync(
            PutPackageRequest model)
        {
            var metadata = await _packageReader.ExecuteAsync(model.Path);
            var storePath = CreateStoreStructure(metadata, _options.GetAbsoluteRootDirectory());

            using (var source = File.Open(model.Path, FileMode.Open))
            using (var destination = File.Create(storePath))
            {
                await source.CopyToAsync(destination);
            }

            return Response.Empty();
        }

        static string CreateStoreStructure(dynamic package, string rootDirectory)
        {
            var name = (string) package.metadata.id;
            var version = package.metadata.version;

            var storePath = name.Split('.')
                .Aggregate(rootDirectory, Path.Combine);
            if (!Directory.Exists(storePath)) Directory.CreateDirectory(storePath);

            return
                Path.Combine(storePath,
                    string.Format("{0}.{1}{2}", name, version, Package.EXTN));
        }
    }
}