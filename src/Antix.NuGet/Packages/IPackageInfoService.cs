using Antix.Services;

namespace Antix.NuGet.Packages
{
    public interface IPackageInfoService :
        IServiceInOut<string, PackageInfo>
    {
    }
}