using System;
using System.Collections.Generic;
using Antix.NuGet.Packages;
using Antix.NuGet.Packages.Models;

namespace Antix.NuGet.Application.Packages.Models
{
    public struct FileSystemPackageMetadata :
        IPackageMetadata
    {
        readonly string _path;

        public static FileSystemPackageMetadata 
            Empty = new FileSystemPackageMetadata();

        readonly string _id;
        readonly string _version;
        readonly DateTimeOffset _createdOn;
        readonly string _hash;
        readonly string _hashAlgorithm;

        readonly string _title;
        readonly string _summary;
        readonly string _copyright;
        readonly string _dependencies;
        readonly string _tags;
        readonly string _description;
        readonly string _releaseNotes;
        readonly string _iconUrl;

        public FileSystemPackageMetadata(
            PackageInfo info) : this()
        {
            _path = info.Path;
            _createdOn = info.CreatedOn;
            _hash = info.Hash;
            _hashAlgorithm = info.HashAlgoritm;

            _id = info.NuSpec.metadata.id.ToLower();
            _version = info.NuSpec.metadata.version.ToLower();
            _title = info.NuSpec.metadata.title;
            _summary = info.NuSpec.metadata.summary;
            _copyright = info.NuSpec.metadata.copyright;
            _tags = info.NuSpec.metadata.tags;
            _description = info.NuSpec.metadata.description;
            _releaseNotes = info.NuSpec.metadata.releaseNotes;
            _iconUrl = info.NuSpec.metadata.iconUrl;
            
            _dependencies = GetDependencies(info.NuSpec.metadata);
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

        public string IconUrl
        {
            get { return _iconUrl; }
        }

        public string Hash
        {
            get { return _hash; }
        }

        public string HashAlgorithm
        {
            get { return _hashAlgorithm; }
        }

        public DateTimeOffset CreatedOn
        {
            get { return _createdOn; }
        }

        public bool IsEmpty()
        {
            return Equals(Empty);
        }
    }
}