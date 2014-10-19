using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Antix.IO;
using Antix.Logging;
using Antix.NuGet.Application.Packages.Storage;
using Antix.NuGet.Packages;
using Antix.Xml;
using Moq;
using Xunit;

namespace Antix.NuGet.Tests.Packages.Storage
{
    public class FileSystemPackageMetadataStoreTest
    {
        const string DirectoryPath = "path";
        readonly string _filePath = Path.Combine(DirectoryPath, "file");
        readonly string _fileNewPath = Path.Combine(DirectoryPath, "file-new");

        [Fact]
        public void when_package_added()
        {
            var fileSystemMonitor = GetChangeMonitor();

            var service = GetService(fileSystemMonitor);

            fileSystemMonitor.Changed(new FileSystemChangedEvent
            {
                Path = _filePath,
                Type = FileSystemChangedEventType.AddedOrUpdated
            });

            Assert.Equal(1, service.Items.Count());
        }

        IFileSystemPackageMetadataStore GetService(
            IFileSystemChangeMonitor fileSystemMonitor)
        {
            return new FileSystemPackageMetadataStore(
                Log.ToConsole,
                GetStorageSettingsMock().Object,
                GetPackageReaderMock().Object,
                fileSystemMonitor
                );
        }

        [Fact]
        public void when_package_deleted()
        {
            var fileSystemMonitor = GetChangeMonitor();

            var service = GetService(fileSystemMonitor);

            fileSystemMonitor.Changed(new FileSystemChangedEvent
            {
                Path = _filePath,
                Type = FileSystemChangedEventType.AddedOrUpdated
            });
            fileSystemMonitor.Changed(new FileSystemChangedEvent
            {
                Path = _filePath,
                Type = FileSystemChangedEventType.Deleted
            });

            Assert.Equal(0, service.Items.Count());
        }

        [Fact]
        public void when_package_renamed()
        {
            var fileSystemMonitor = GetChangeMonitor();

            var service = GetService(fileSystemMonitor);

            fileSystemMonitor.Changed(new FileSystemChangedEvent
            {
                Path = _filePath,
                Type = FileSystemChangedEventType.AddedOrUpdated
            });
            fileSystemMonitor.Changed(new FileSystemChangedEvent
            {
                Path = _filePath,
                NewPath = _fileNewPath,
                Type = FileSystemChangedEventType.Renamed
            });

            Assert.Equal(1, service.Items.Count());
        }

        [Fact]
        public void when_package_folder_removed()
        {
            var fileSystemMonitor = GetChangeMonitor();

            var service = GetService(fileSystemMonitor);

            fileSystemMonitor.Changed(new FileSystemChangedEvent
            {
                Path = _filePath,
                Type = FileSystemChangedEventType.AddedOrUpdated
            });
            fileSystemMonitor.Changed(new FileSystemChangedEvent
            {
                Path = DirectoryPath,
                Type = FileSystemChangedEventType.Deleted
            });

            Assert.Equal(0, service.Items.Count());
        }

        static Mock<IFileSystemStorageSettings> GetStorageSettingsMock()
        {
            var mock = new Mock<IFileSystemStorageSettings>();
            mock.SetupAllProperties();

            return mock;
        }

        static Mock<IPackageReader> GetPackageReaderMock()
        {
            var mock = new Mock<IPackageReader>();
            mock.Setup(o => o.ExecuteAsync(It.IsAny<string>()))
                .Returns(
                    () => Task.FromResult(DynamicXml.Parse(@"<package><metadata><id>hello</id></metadata></package>")));

            return mock;
        }

        static IFileSystemChangeMonitor GetChangeMonitor()
        {
            Action<FileSystemChangedEvent> change = null;

            var mock = new Mock<IFileSystemChangeMonitor>();
            mock
                .Setup(o => o.OnChanged(
                    It.IsAny<Action<FileSystemChangedEvent>>(),
                    It.IsAny<FileSystemChangeMonitorOptions>())
                )
                .Callback((Action<FileSystemChangedEvent> a, FileSystemChangeMonitorOptions o)
                    => { change = a; });

            mock
                .Setup(o => o.Changed(It.IsAny<FileSystemChangedEvent>()))
                .Callback((FileSystemChangedEvent e) => change(e));

            return mock.Object;
        }
    }
}