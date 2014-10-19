using Antix.Services;
using Antix.Xml;

namespace Antix.NuGet.Packages
{
    public interface IPackageReader :
        IServiceInOut<string, DynamicXml>
    {
    }
}