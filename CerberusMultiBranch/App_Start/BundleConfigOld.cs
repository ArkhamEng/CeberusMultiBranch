using System.Web;
using System.Web.Optimization;

namespace CerberusMultiBranch
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/bundles/Styles").Include(
                    "~/Vendor/bootstrap/css/bootstrap.min.css",
                    "~/Vendor/font-awesome/css/font-awesome.min.css",
                
                    "~/Vendor/pnotify/css/pnotify.css",
                    "~/Vendor/pnotify/css/pnotify.buttons.css",
                    "~/Vendor/pnotify/css/pnotify.nonblock.css",
                    "~/Content/Site.css"));

            //DATA TABLE STYLE
          /*  bundles.Add(new StyleBundle("~/bundles/DataTableStyles").Include(
                     "~/Vendor/datatables.net/css/jquery.dataTables.min.css",
                      "~/Vendor/datatables.net/css/dataTables.bootstrap.min.css",
                      "~/Vendor/datatables.net/css/buttons.bootstrap.min.css",
                      "~/Vendor/datatables.net/css/fixedHeader.bootstrap.min.css",
                      "~/Vendor/datatables.net/css/fixedColumns.dataTables.min.css",
                      "~/Vendor/datatables.net/css/responsive.bootstrap.min.css",
                      "~/Vendor/datatables.net/css/scroller.bootstrap.min.css"));*/


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-2.6.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/JqueryScripts").Include(
                "~/Vendor/jquery/js/_jquery-ui-1.12.1.min.js",
                "~/Vendor/jquery/js/jquery-3.1.1.js",
                "~/Vendor/jquery/js/jquery.maskedinput.min.js"));

         
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                      "~/Vendor/jquery/js/jquery.validate.min.js",
                      "~/Vendor/jquery/js/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/PluginScripts").Include(
                       "~/Vendor/bootstrap/js/bootstrap.min.js",
                       "~/Vendor/pnotify/js/pnotify.js",
                        "~/Vendor/pnotify/js/pnotify.buttons.js",
                         "~/Vendor/pnotify/js/pnotify.nonblock.js"));

            //DATA TABLE SCRIPTS
       /*     bundles.Add(new ScriptBundle("~/bundles/DataTableScripts").Include(
            "~/Vendor/datatables.net/js/jquery.dataTables.min.js",
            "~/Vendor/datatables.net/js/dataTables.bootstrap.min.js",
            "~/Vendor/datatables.net/js/dataTables.keyTable.min.js",
             "~/Vendor/datatables.net/js/dataTables.fixedHeader.min.js",
            "~/Vendor/datatables.net/js/dataTables.fixedColumns.min.js",
            "~/Vendor/pdfmake/build/pdfmake.min.js",
            "~/Vendor/pdfmake/build/vfs_fonts.js",
            "~/Vendor/jszip/dist/jszip.min.js",
            "~/Vendor/datatables.net/js/dataTables.buttons.min.js",
            "~/Vendor/datatables.net/js/buttons.bootstrap.min.js",
            "~/Vendor/datatables.net/js/buttons.flash.min.js",
            "~/Vendor/datatables.net/js/buttons.html5.min.js",
            "~/Vendor/datatables.net/js/buttons.colVis.min.js",
            "~/Vendor/datatables.net/js/buttons.print.min.js",
            "~/Vendor/datatables.net/js/dataTables.responsive.min.js",
            "~/Vendor/datatables.net/js/responsive.bootstrap.js",
            "~/Vendor/datatables.net/js/dataTables.scroller.min.js"));*/

            bundles.Add(new ScriptBundle("~/bundles/CustomScripts").Include(
                 "~/AppScripts/Global.js"));

        }
    }
}
