using System;

namespace Antix.NuGet.Packages.Models
{
    public interface IPackageMetadata
    {
        string Id { get; }
        string Version { get; }
        DateTimeOffset Created { get; }
        string MD5Hash { get; }

        string Title { get; }
        string Summary { get; }
        string Copyright { get; }
        string Dependencies { get; }
        string Tags { get; }
        string Description { get; }
        string ReleaseNotes { get; }
    }
}