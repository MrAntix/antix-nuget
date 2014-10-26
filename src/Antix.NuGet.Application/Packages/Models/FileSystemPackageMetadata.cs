using System;
using System.Collections.Generic;
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
        readonly string _copyright;
        readonly string _dependencies;
        readonly string _tags;
        readonly string _description;
        readonly string _releaseNotes;
        object _getDependency;

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
            _copyright = package.metadata.copyright;
            _tags = package.metadata.tags;
            _description = package.metadata.description;
            _releaseNotes = package.metadata.releaseNotes;

            _dependencies = GetDependencies(package.metadata);
        }

        static string GetDependencies(dynamic metadata)
        {
            if (metadata.dependencies == null) return string.Empty;

            var dependencies = new List<string>();
            foreach (var dependency in metadata.dependencies.dependency)
            {
                dependencies.Add(GetDependency(dependency));
            }

            return string.Join("|", dependencies);
        }

        static string GetDependency(dynamic dependency)
        {
            return string.Format("{0}:{1}:", dependency.id, dependency.version);
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

        public string Copyright
        {
            get { return _copyright; }
        }

        public string Dependencies
        {
            get { return _dependencies; }
        }

        public string Tags
        {
            get { return _tags; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string ReleaseNotes
        {
            get { return _releaseNotes; }
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