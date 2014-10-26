using System;
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
        readonly DateTimeOffset _created;
        readonly string _md5Hash;

        readonly string _title;
        readonly string _summary;

        public FileSystemPackageMetadata(
            string path, 
            dynamic package, 
            DateTimeOffset created,
            string md5Hash) : this()
        {
            _path = path;
            _id = package.metadata.id.ToLower();
            _version = package.metadata.version.ToLower();
            _created = created;
            _md5Hash = md5Hash;

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

        public string MD5Hash
        {
            get { return _md5Hash; }
        }

        public DateTimeOffset Created
        {
            get { return _created; }
        }

        public bool IsEmpty()
        {
            return Equals(Empty);
        }
    }
}