using System;
using System.IO;

namespace Antix.NuGet.Packages.Models
{
    public class Package
    {
        readonly IPackageMetadata _metadata;
        readonly Stream _stream;

        public const string EXTN = ".nupkg";
        public const string EXTN_PATTERN = "*" + EXTN;

        public Package(
            IPackageMetadata metadata, Stream stream)
        {
            _metadata = metadata;
            _stream = stream;
        }

        public IPackageMetadata Metadata
        {
            get { return _metadata; }
        }

        public string MD5
        {
            get { return _metadata.Hash; }
        }

        public Stream Stream
        {
            get { return _stream; }
        }

        public DateTimeOffset Created {
            get { return _metadata.CreatedOn; }
        }
    }
}