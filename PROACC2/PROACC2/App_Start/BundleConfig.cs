using System.Web;
using System.Web.Optimization;

namespace PROACC2
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Default
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"
            //          ));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"
            //         ));


            //Local --Script
            bundles.Add(new ScriptBundle("~/bundles/Scripts/jquery").Include(
                        "~/Assets/js/jquery-1.12.4.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/bootstrap").Include(
                       "~/Assets/lib/popper.js/popper.min.js",
                       "~/Assets/lib/bootstrap/js/bootstrap.min.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/Layout").Include(
                        "~/Assets/lib/metisMenu/metisMenu.min.js",
                        "~/Assets/js/app.js",
                        "~/Assets/Select2/select2.min.js",
                        //"~/Assets/DataTable/jquery.dataTables.min.js",
                       //"~/Assets/PdfMaker/pdfmake.min.js",
                       "~/Assets/Select2/vfs_fonts.js"
                       // "~/Assets/DataTable/bootstrap-datepicker.min.js",
                        //"~/Assets/DataTable/bootstrap-multiselect.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/multiselect").Include(
                "~/assets/lib/MultiSelect/bootstrap-multiselect.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/DatePicker").Include(
                "~/assets/lib/Jquery-UI/jquery-ui.js",
                "~/assets/lib/Jquery-UI/jquery-ui.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/ChartJS").Include(
                "~/assets/js/Chart/Chart.js",
                "~/assets/js/Chart/Chart.RadialGauge.umd.js",
                "~/assets/js/Chart/chartjs-plugin-labels.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/AmCharts").Include(
                       "~/assets/js/Chart/Amcharts/core.js",
                       "~/assets/js/Chart/Amcharts/charts.js",
                       "~/assets/js/Chart/Amcharts/timeline.js",
                        "~/assets/js/Chart/Amcharts/bullets.js",
                       "~/assets/js/Chart/Amcharts/sunburst.js",
                       "~/assets/js/Chart/Amcharts/animated.js"
                       ));
            bundles.Add(new ScriptBundle("~/bundles/Scripts/Error").Include(
                "~/assets/js/ErrorScript.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/notiflix").Include(
               "~/Assets/Notiflix/notiflix-2.3.1.min.js",
               "~/Assets/Notiflix/notiflix-aio-2.3.1.min.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/moment").Include(
               "~/assets/js/moment.min.js",
               "~/assets/js/moment-timezone-with-data.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/Error").Include(
               "~/assets/js/ErrorScript.js"
               ));


            //LOGIN PAGE
            bundles.Add(new ScriptBundle("~/bundles/Scripts").Include(
               "~/assets/js/scripts/App/*.js"
               ));



            //Local --StyleBundle
            //bundles.Add(new StyleBundle("~/Content/css/Layout").Include(
            //          "~/assets/lib/font-awesome/css/font-awesome.min.css",
            //          "~/assets/lib/bootstrap/css/bootstrap.min.css",
            //          "~/assets/css/main.css",
            //           "~/assets/Select2/select2.min.css"
            //           //"~/Assets/DataTable/bootstrap-datepicker.min.css", 
            //          // "~/Assets/DataTable/bootstrap-multiselect.css"
            //          ));

            bundles.Add(new StyleBundle("~/Content/css/Layout")
                .Include("~/assets/lib/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform())
                .Include("~/assets/lib/bootstrap/css/bootstrap.min.css")
                .Include("~/assets/css/mainNew.css", new CssRewriteUrlTransform())
                .Include(
                      "~/assets/Select2/select2.min.css"
                     //"~/Assets/DataTable/bootstrap-datepicker.min.css", 
                     // "~/Assets/DataTable/bootstrap-multiselect.css"
                     ));
            bundles.Add(new StyleBundle("~/Content/css/notiflix").Include("~/Assets/Notiflix/notiflix-2.3.1.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/multiselect").Include(
                "~/assets/lib/MultiSelect/bootstrap-multiselect.css"
                ));

                 bundles.Add(new StyleBundle("~/Content/css/DatePicker").Include(
                "~/assets/lib/Jquery-UI/jquery-ui.css",
                "~/assets/lib/Jquery-UI/jquery-ui.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css/ErrorStyle").Include(
               "~/assets/css/ErrorStyles.css"
               ));

            BundleTable.EnableOptimizations = false;

        }

//        var styles = new StyleBundle("~/styles")
//    .Include("~/content/pages/meetings.min.css", new CssRewriteUrlTransform())
//    .Include("~/content/pages/login.min.css", new CssRewriteUrlTransform())
//    .Include("~/content/pages/error.min.css", new CssRewriteUrlTransform())
//    .Include("~/content/Site.css", new CssRewriteUrlTransform());
//        styles.Transforms.Insert(0, new StyleRelativePathTransform());
//styles.Orderer = new AsIsBundleOrderer();
//        bundles.Add(styles);


        //public class StyleRelativePathTransform : IBundleTransform
        //{
        //    private static Regex pattern = new Regex(@"url\s*\(\s*([""']?)([^:)]+)\1\s*\)", RegexOptions.IgnoreCase); public void Process(BundleContext context, BundleResponse response)
        //    {
        //        response.Content = string.Empty; foreach (BundleFile file in response.Files)
        //        {
        //            using (var reader = new StreamReader(file.VirtualFile.Open()))
        //            {
        //                var contents = reader.ReadToEnd();
        //                var matches = pattern.Matches(contents); if (matches.Count == 0)
        //                {
        //                    continue;
        //                }
        //                var directoryPath = VirtualPathUtility.GetDirectory(file.VirtualFile.VirtualPath);

        //                foreach (Match match in matches)
        //                {
        //                    var fileRelativePath = match.Groups[2].Value;
        //                    var fileVirtualPath = VirtualPathUtility.Combine(directoryPath, fileRelativePath);
        //                    var quote = match.Groups[1].Value;
        //                    var replace = String.Format("url({0}{1}{0})", quote, VirtualPathUtility.ToAbsolute(fileVirtualPath)); contents = contents.Replace(match.Groups[0].Value, replace);
        //                }
        //                response.Content = String.Format("{0}\r\n{1}", response.Content, contents);
        //            }
        //        }
        //    }
        //}
        //public class AsIsBundleOrderer : IBundleOrderer
        //{
        //    public virtual IEnumerable<FileInfo> OrderFiles(BundleContext context, IEnumerable<FileInfo> files)
        //    {
        //        return files;
        //    }
        //    public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        //    {
        //        return files;
        //    }
        //}
    }
}
