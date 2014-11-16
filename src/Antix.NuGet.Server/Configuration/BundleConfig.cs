using System.Web.Optimization;

namespace Antix.NuGet.Server.Configuration
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles
                .Add(new ScriptBundle("~/bundles/scripts")
                    .Include("~/Scripts/angular.js")
                    .Include("~/Scripts/angular-resource.js")
                    .Include("~/Scripts/angular-touch.js")
                    .Include("~/Scripts/angular-cookies.js")
                    .Include("~/Scripts/angular-animate.js")
                    .Include("~/Scripts/angular-sanitize.js")
                    .Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js")
                    .Include("~/Scripts/angularUI/ui-router.js")
                    .Include("~/Scripts/jquery-{version}.js")
                    .Include("~/Scripts/jquery.signalR-{version}.js")
                    .IncludeDirectory("~/Scripts/antix/", "*.js", true)
                    .IncludeDirectory("~/client/", "*.js", true)
                );

            bundles
                .Add(new StyleBundle("~/bundles/styles")
                    .Include("~/Content/bootstrap.css")
                    .Include("~/Content/animate.css")
                    .IncludeDirectory("~/Scripts/antix/", "*.css", true)
                    .Include("~/Content/site.css")
                    .IncludeDirectory("~/Content/site/", "*.css", true)
                );

        }
    }
}