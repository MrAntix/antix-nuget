using System;
using Antix.Services;

namespace Antix.NuGet.API.Packages
{
    public interface IPackagesSettings :
        IService
    {
        string PackageRoot { get; }
        Guid APIKey { get; }
    }
}