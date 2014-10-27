using System.Collections.Generic;
using Antix.NuGet.Packages.Models;

namespace Antix.NuGet.Application.Packages.Storage
{
    public interface IFileSystemPackageMetadataStore
    {
        IEnumerable<IPackageMetadata> Items { get; }
    }
}