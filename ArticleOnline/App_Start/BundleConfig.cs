using System.Web;
using System.Web.Optimization;

namespace ArticleOnline
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/style").Include(
            "~/Content/css/style.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
            "~/Content/js/main.js"));
        }
    }
}
