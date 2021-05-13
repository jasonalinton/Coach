using System.Web;
using System.Web.Optimization;

namespace Coach.Site
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* Scripts */
            bundles.Add(new ScriptBundle("~/bundles/js/jquery").Include(
                        "~/Scripts/jQuery/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/js/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            /* Bootstrap JS */
            bundles.Add(new ScriptBundle("~/bundles/js/bootstrap").Include(
                      "~/Scripts/Bootstrap/bootstrap.min.js"));

            /* Kendo UI JS */
            bundles.Add(new ScriptBundle("~/bundles/js/kendoUI").Include(
                        "~/Scripts/KendoUI/kendo.all.min.js"));

            /* Kendo UI JS - OG */
            bundles.Add(new ScriptBundle("~/bundles/js/kendoUIOG").Include(
                      "~/Scripts/KendoUI/OG/kendo.all.min.js",
                      "~/Scripts/KendoUI/OG/kendo.timezones.min.js"));

            /* Global JS */
            bundles.Add(new ScriptBundle("~/bundles/js/global").Include(
                        "~/Scripts/Misc/date.js",
                        "~/Scripts/site.js"));

            /* Bootstrap CSS */
            bundles.Add(new StyleBundle("~/bundles/css/bootstrap").Include(
                      "~/Content/Bootstrap/bootstrap.min.css"));

            /* Font Awesome CSS */
            bundles.Add(new StyleBundle("~/bundles/css/fontAwesome").Include(
                      "~/Content/FontAwesome/css/all.min.css",
                      "~/Content/FontAwesome/webfonts"));

            /* Kendo UI CSS */
            bundles.Add(new StyleBundle("~/bundles/css/kendoUI").Include(
                      "~/Content/KendoUI/Bootstrap",
                      "~/Content/KendoUI/kendo.common.min.css",
                      "~/Content/KendoUI/kendo.bootstrap-v4.min.css",
                      "~/Content/KendoUI/kendo.bootstrap.mobile.min.css"));

            /* Kendo UI CSS - OG */
            bundles.Add(new StyleBundle("~/bundles/css/kendoUIOG").Include(
                      "~/Content/KendoUI/OG/kendo.common.min.css",
                      "~/Content/KendoUI/OG/kendo.default.min.css",
                      "~/Content/KendoUI/OG/kendo.default.mobile.min.css",
                      "~/Content/KendoUI/OG/kendo.material.min.css",
                      "~/Content/KendoUI/OG/kendo.mobile.all.min.css",
                      "~/Content/KendoUI/OG/kendo-modal.css"));

            /* Global CSS */
            bundles.Add(new StyleBundle("~/bundles/css/global").Include(
                      "~/Content/jQuery/jquery-ui.min.css",
                      "~/Content/site.css"));

            BundleTable.EnableOptimizations = false;

        }
    }
}
