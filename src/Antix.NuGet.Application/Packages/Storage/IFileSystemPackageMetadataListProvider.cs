using System.Collections.Generic;
using Antix.NuGet.Packages.Models;
using Antix.Services;

namespace Antix.NuGet.Application.Packages.Storage
{
    public interface IFileSystemPackageMetadataListProvider :
        IServiceOut<IList<IPackageMetadata>>
    {
    }
}