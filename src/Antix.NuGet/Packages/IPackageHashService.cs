using Antix.Services;

namespace Antix.NuGet.Packages
{
    public interface IPackageHashService :
        IServiceInOut<string, string>
    {
        string Algorithm { get; }
    }
}