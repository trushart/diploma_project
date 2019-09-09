using System.Web;
using System.Web.Optimization;

namespace FIT.Diploma.WebApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/daterangepicker.css",
                      "~/Content/font-awesome.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/leagueinfo").Include(
                        "~/Scripts/Custom/leagueinfo.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/predictions").Include(
                        "~/Scripts/Custom/predictions.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/searchingtool").Include(
                        "~/Scripts/moment.js",
                        "~/Scripts/daterangepicker.js",
                        "~/Scripts/Custom/searchingtool.js"
                        ));
        }
    }
}
