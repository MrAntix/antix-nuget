﻿using System;

namespace Antix.NuGet.Packages.Models
{
    public interface IPackageMetadata
    {
        string Id { get; }
        string Version { get; }
        DateTimeOffset CreatedOn { get; }
        string Hash { get; }
        string HashAlgorithm { get; }

        string Title { get; }
        string Summary { get; }
        string Copyright { get; }
        string Dependencies { get; }
        string Tags { get; }
        string Description { get; }
        string ReleaseNotes { get; }
        string IconUrl { get; }
        string ProjectUrl { get; }
        string LicenceUrl { get; }
    }
}