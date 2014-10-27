using System;

namespace Antix.NuGet.Packages
{
    public class PackageInfo
    {
        public string Path { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public dynamic NuSpec { get; set; }

        public string Hash { get; set; }
        public string HashAlgoritm { get; set; }

        public long SizeInBytes { get; set; }
    }
}