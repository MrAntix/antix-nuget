using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Antix.Xml;

namespace Antix.NuGet.Packages
{
    public class PackageInfoService :
        IPackageInfoService
    {
        public const string NUSPEC_EXTN = ".nuspec";
        readonly IPackageHashService _packageHashService;

        public PackageInfoService(
            IPackageHashService packageHashService)
        {
            _packageHashService = packageHashService;
        }


        public async Task<PackageInfo> ExecuteAsync(string path)
        {
            DynamicXml xml;
            using (var arch = ZipFile.OpenRead(path))
            {
                var nuspec = arch.Entries.Single(e => e.Name.EndsWith(NUSPEC_EXTN));
                xml = DynamicXml.Load(nuspec.Open());
            }

            var info = new FileInfo(path);
            var hash = await _packageHashService.ExecuteAsync(path);
            return new PackageInfo
            {
                Path = path,
                CreatedOn = info.CreationTimeUtc,
                SizeInBytes = info.Length,
                Hash = hash,
                HashAlgoritm = _packageHashService.Algorithm,
                NuSpec = xml
            };
        }
    }
}