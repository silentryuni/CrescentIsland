using System.Web;
using System.Web.Optimization;

namespace CrescentIsland.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/Assets/Scripts/jquery-1.11.3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Assets/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Assets/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/mainjs").Include(
                    "~/Assets/Scripts/bootstrap.min.js",
                    "~/Assets/Scripts/respond.js",
                    "~/Assets/Scripts/grids.min.js",
                    "~/Assets/Scripts/functions.js",
                    "~/Assets/Scripts/onload.js"));

            var lessBundle = new StyleBundle("~/styles/maincss").Include(
                    "~/Assets/Styles/reset.less",
                    "~/Assets/Styles/variables.less",
                    "~/Assets/Styles/bootstrap.css",
                    "~/Assets/Styles/font-awesome.css",
                    "~/Assets/Styles/global.less",
                    "~/Assets/Styles/header.less",
                    "~/Assets/Styles/main.less",
                    "~/Assets/Styles/sprite-icons.less");
            lessBundle.Transforms.Add(new LessTransform());
            lessBundle.Transforms.Add(new CssMinify());

            bundles.Add(lessBundle);

            BundleTable.EnableOptimizations = true;
        }
    }

    public class LessTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            response.Content = dotless.Core.Less.Parse(response.Content);
            response.ContentType = "text/css";
        }
    }
}
