using System;
using Antix.Http.Filters;

namespace Antix.NuGet.API.Packages.Filters
{
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Method,
        AllowMultiple = false, Inherited = true)]
    public class APIKeyAttribute :
        Attribute, IFilterServiceAttribute
    {
        readonly Type _serviceType;

        public APIKeyAttribute()
        {
            _serviceType = typeof (APIKeyFilter);
        }

        public Type ServiceType
        {
            get { return _serviceType; }
        }
    }
}