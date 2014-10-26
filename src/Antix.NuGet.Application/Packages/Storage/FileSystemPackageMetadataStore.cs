using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Antix.IO;
using Antix.Logging;
using Antix.NuGet.Application.Packages.Models;
using Antix.NuGet.Packages;
using Antix.NuGet.Packages.Models;

namespace Antix.NuGet.Application.Packages.Storage
{
    public class FileSystemPackageMetadataStore :
        IFileSystemPackageMetadataStore
    {
        readonly Log.Delegate _log;
        readonly IPackageReader _packageReader;
        readonly MD5CryptoServiceProvider _hashService = new MD5CryptoServiceProvider();

        readonly IList<FileSystemPackageMetadata> _items;

        public FileSystemPackageMetadataStore(
            Log.Delegate log,
            IFileSystemStorageSettings settings,
            IPackageReader packageReader,
            IFileSystemChangeMonitor fileSystemChangeMonitor)
        {
            _log = log;
            _packageReader = packageReader;

            _items = new List<FileSystemPackageMetadata>();

            var rootDirectory = settings.GetAbsoluteRootDirectory();
            Initialize(rootDirectory);

            fileSystemChangeMonitor
                .OnChanged(FileChanged,
                    new FileSystemChangeMonitorOptions
                    {
                        Directory = rootDirectory,
                        Pattern = Package.EXTN_PATTERN,
                        IncludeSubdirectories = true
                    });
        }

        public IEnumerable<IPackageMetadata> Items
        {
            get { return _items.Cast<IPackageMetadata>(); }
        }

        void FileChanged(FileSystemChangedEvent e)
        {
            _log.Debug(m => m("FileChange {0}", e));

            switch (e.Type)
            {
                case FileSystemChangedEventType.AddedOrUpdated:
                    Add(e.Path);
                    break;
                case FileSystemChangedEventType.Deleted:
                    Remove(e.Path);
                    break;
                case FileSystemChangedEventType.Renamed:
                    Remove(e.Path);
                    Add(e.NewPath);
                    break;
            }
        }

        void Add(string path)
        {
            if (path == null) throw new ArgumentNullException("path");

            Remove(path);

            var info = new FileInfo(path);
            string hash64;

            using (var stream = info.OpenRead())
            {
                var hash = _hashService.ComputeHash(stream);
                hash64 = Convert.ToBase64String(hash);
            }

            var packageMetadata = new FileSystemPackageMetadata(
                path, 
                _packageReader.ExecuteAsync(path).Result,
                new DateTimeOffset(info.CreationTimeUtc),
                hash64
                );


            _log.Debug(
                m => m("Adding package {0} version {1}",
                    packageMetadata.Id, packageMetadata.Version));

            _items.Add(packageMetadata);
        }

        void Remove(string path)
        {
            if (path == null) throw new ArgumentNullException("path");

            var items = _items
                .Where(i => i.Path.StartsWith(path))
                .ToArray();
            if (!items.Any()) return;

            foreach (var item in items)
            {
                _log.Debug(m => m("Removing {0}", item.Path));
                _items.Remove(item);
            }
        }

        void Initialize(string rootDirectory)
        {
            if (rootDirectory == null) return;

            if (!Directory.Exists(rootDirectory))
                Directory.CreateDirectory(rootDirectory);

            else
            {
                Task.Run(() =>
                {
                    foreach (var file in Directory
                        .GetFiles(rootDirectory, Package.EXTN_PATTERN,
                            SearchOption.AllDirectories))
                    {
                        FileChanged(new FileSystemChangedEvent
                        {
                            Path = file,
                            Type = FileSystemChangedEventType.AddedOrUpdated
                        });
                    }
                });
            }
        }
    }
}