using Antix.Services;

namespace Antix.NuGet.Application.Packages.Storage
{
    public interface IFileSystemStorageSettings :
        IService
    {
        string RootDirectory { get; }
        string GetAbsoluteRootDirectory();
    }
}