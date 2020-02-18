using System.Web;
using System.Web.Optimization;

namespace CerberusMultiBranch
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
          
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/_jquery-ui-1.12.1.min.js",
                        "~/Scripts/jquery-3.1.1.js",
                        "~/Scripts/jquery.maskedinput.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/imageEditor").Include(
              "~/Scripts/cropper/cropper.min.js",
                      "~/Scripts/cropper/jquery-cropper.min.js",
                      "~/AppScripts/Controls/ImageEditor.js"));


            bundles.Add(new ScriptBundle("~/bundles/pnotify").Include(
                "~/Scripts/pnotify/pnotify.js",
                "~/Scripts/pnotify/pnotify.buttons.js",
                "~/Scripts/pnotify/pnotify.nonblock.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                    "~/Scripts/Common.js",
                    "~/AppScripts/Global.js",
                    "~/AppScripts/Catalog.js",
                    "~/AppScripts/Operative.js"));


          // Use the development version of Modernizr to develop with and learn from. Then, when you're
          // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
          bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/pnotifycss").Include(
                   "~/Content/pnotify/pnotify.css",
                   "~/Content/pnotify/pnotify.buttons.css",
                   "~/Content/pnotify/pnotify.nonblock.css"));


            bundles.Add(new ScriptBundle("~/bundles/PurchaseOrder").Include(
                    "~/AppScripts/PurchaseOrder.js"));

            bundles.Add(new ScriptBundle("~/bundles/PurchaseEstimation").Include(
                   "~/AppScripts/PurchaseEstimation.js"));

            bundles.Add(new ScriptBundle("~/bundles/SaleOrder").Include(
                "~/AppScripts/SaleOrder.js"));

            #region Search Scripts
            bundles.Add(new ScriptBundle("~/bundles/SearchCustomer").Include(
              "~/AppScripts/Controls/SearchCustomer.js"));

            bundles.Add(new ScriptBundle("~/bundles/SearchProduct").Include(
            "~/AppScripts/Controls/SearchProduct.js"));

            bundles.Add(new ScriptBundle("~/bundles/Searches").Include(
             "~/AppScripts/Searches.js"));

            #endregion
        }
    }
}
