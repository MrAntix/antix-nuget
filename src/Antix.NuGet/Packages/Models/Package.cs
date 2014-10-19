using System.IO;

namespace Antix.NuGet.Packages.Models
{
    public class Package
    {
        readonly IPackageMetadata _metadata;
        readonly Stream _data;

        public const string EXTN = ".nupkg";
        public const string EXTN_PATTERN = "*" + EXTN;

        public Package(
            IPackageMetadata metadata, Stream data)
        {
            _metadata = metadata;
            _data = data;
        }

        public IPackageMetadata Metadata
        {
            get { return _metadata; }
        }
    }
}