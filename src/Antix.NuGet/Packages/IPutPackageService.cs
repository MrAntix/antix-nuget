using Antix.NuGet.Packages.Models;
using Antix.Services;
using Antix.Services.Models;

namespace Antix.NuGet.Packages
{
    public interface IPutPackageService :
        IServiceInOut<PutPackageRequest, IServiceResponse>
    {
    }
}