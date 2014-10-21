using Antix.NuGet.Packages.Models;

namespace Antix.NuGet.Application.Packages.Models
{
    public struct FileSystemPackageMetadata :
        IPackageMetadata
    {
        readonly string _path;
        public static FileSystemPackageMetadata Empty = new FileSystemPackageMetadata();

        readonly string _id;
        readonly string _version;
        readonly string _title;
        readonly string _summary;

        public FileSystemPackageMetadata(
            string path,
            dynamic package) : this()
        {
            _path = path;
            _id = package.metadata.id;
            _version = package.metadata.version;
            _title = package.metadata.title;
            _summary = package.metadata.summary;
        }

        public string Path
        {
            get { return _path; }
        }

        public string Id
        {
            get { return _id; }
        }

        public string Version
        {
            get { return _version; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Summary
        {
            get { return _summary; }
        }

        public bool IsEmpty()
        {
            return Equals(Empty);
        }
    }
}