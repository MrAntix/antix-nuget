using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Antix.Http.Filters;
using Antix.Services;

namespace Antix.NuGet.API.Packages.Filters
{
    public class APIKeyFilter :
        FilterServiceBase<APIKeyAttribute>,
        IActionFilter, IService
    {
        public const string HEADER_NAME = "X-NuGet-ApiKey";
        readonly IPackagesSettings _settings;

        public APIKeyFilter(IPackagesSettings settings)
        {
            _settings = settings;
        }

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            if (_settings.APIKey.Equals(Guid.Empty)) return continuation();

            if (actionContext.Request.Headers.Contains(HEADER_NAME))
            {
                Guid actualApiKey;
                if (Guid.TryParse(
                    actionContext.Request.Headers.GetValues(HEADER_NAME).Single(),
                    out actualApiKey)
                    && _settings.APIKey.Equals(actualApiKey))
                {
                    return continuation();
                }
            }

            throw new HttpResponseException(HttpStatusCode.Forbidden);
        }
    }
}