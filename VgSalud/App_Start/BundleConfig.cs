using System.Web;
using System.Web.Optimization;

namespace VgSalud
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/plugins/jQuery/jquery-2.2.3.min.js",
                        "~/Scripts/jquery-1.10.2.js",
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/plugins/morris/raphael-min.js",
                        "~/plugins/morris/morris.min.js",
                        "~/plugins/chartjs/Chart.min.js",
                        "~/Content/Scripts/bootstrap.min.js",
                        "~/Content/Scripts/select2.full.min.js",
                        "~/Content/Scripts/jquery.inputmask.j",
                        "~/plugins/input-mask/jquery.inputmask.js",
                        "~/plugins/input-mask/jquery.inputmask.date.extensions.js",
                        "~/plugins/input-mask/jquery.inputmask.extensions.js",
                        "~/Content/Scripts/moment.min.js",
                        "~/Content/Scripts/daterangepicker.js",
                        "~/Content/Scripts/fastclick.js",
                        "~/Content/Scripts/app.min.js",
                        "~/plugins/timepicker/bootstrap-timepicker.min.js",
                        "~/plugins/iCheck/icheck.min.js",
                        "~/Content/Scripts/demo.js",
                        "~/plugins/datatables/jquery.dataTables.min.js",
                        "~/plugins/datatables/dataTables.bootstrap.min.js",
                        "~/Content/Scripts/moment.min.js",
                        "~/plugins/datepicker/bootstrap-datepicker.js",
                        "~/Content/Scripts/ckeditor.js",
                        "~/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                        "~/Content/Scripts/html5shiv.min.js",
                        "~/Content/Scripts/respond.min.js",
                        "~/plugins/fullcalendar/fullcalendar.min.js",
                      "~/plugins/datepicker/datepicker-esp.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Styles/bootstrap.min.css",
                      "~/Content/Styles/font-awesome.min.css",
                      "~/Content/Styles/ionicons.min.css",
                      "~/Content/Styles/daterangepicker-bs3.css",
                      "~/plugins/iCheck/all.css",
                      "~/Content/Styles/bootstrap-colorpicker.min.css",
                      "~/Content/Styles/bootstrap-timepicker.css",
                      "~/plugins/select2/select2.min.css",
                      "~/Content/Styles/AdminLTE.min.css",
                      "~/Content/Styles/_all-skins.min.css",
                      "~/plugins/daterangepicker/daterangepicker.css",
                      "~/plugins/datepicker/datepicker3.css",
                      "~/plugins/iCheck/all.css",
                      "~/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css",
                      "~/plugins/timepicker/bootstrap-timepicker.min.css",
                      "~/plugins/fullcalendar/fullcalendar.min.css",
                      "~/plugins/fullcalendar/fullcalendar.print.css",
                      "~/plugins/morris/morris.css",
                      "~/Content/Styles/jquery-ui.css"));
        }
    }
}
