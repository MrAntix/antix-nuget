using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR.Hubs;

namespace Antix.NuGet.Application.Hubs
{
    public class JavaScriptMinifier : IJavaScriptMinifier
    {
        public string Minify(string source)
        {
            return new Minifier().MinifyJavaScript(source);
        }
    }
}