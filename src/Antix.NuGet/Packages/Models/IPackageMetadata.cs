namespace Antix.NuGet.Packages.Models
{
    public interface IPackageMetadata
    {
        string Id { get; }
        string Version { get; }
        string Title { get; }
        string Summary { get; }
    }
}