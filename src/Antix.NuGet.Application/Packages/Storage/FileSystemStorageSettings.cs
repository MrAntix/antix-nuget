using System;
using System.IO;

namespace Antix.NuGet.Application.Packages.Storage
{
    public partial class FileSystemStorageSettings : IFileSystemStorageSettings
    {
        public string GetAbsoluteRootDirectory()
        {
            if (RootDirectory.StartsWith("~"))
                return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    RootDirectory.TrimStart('~', '\\'));

            return RootDirectory;
        }
    }
}