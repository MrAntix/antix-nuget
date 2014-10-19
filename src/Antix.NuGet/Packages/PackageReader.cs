using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Antix.Xml;

namespace Antix.NuGet.Packages
{
    public class PackageReader :
        IPackageReader
    {
        public const string NUSPEC_EXTN = ".nuspec";

        public async Task<DynamicXml> ExecuteAsync(string path)
        {
            using (var arch = ZipFile.OpenRead(path))
            {
                var nuspec = arch.Entries.Single(e => e.Name.EndsWith(NUSPEC_EXTN));
                return DynamicXml.Load(nuspec.Open());
            }
        }
    }
}