using Antix.NuGet.Packages.Models;
using Antix.Services;
using Antix.Services.Models;

namespace Antix.NuGet.Packages
{
    public interface IGetPackageService :
        IServiceInOut<GetPackageRequest, Response<Package>>
    {
        
    }
}