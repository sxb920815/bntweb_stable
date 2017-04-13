/* 
    ======================================================================== 
        File name：		BundleProvider
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/17 15:21:48
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System.Web.Optimization;
using BntWeb.UI.Bundle;

namespace BntWeb.HelpCenter
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //Js
            bundles.Add(new ScriptBundle("~/js/admin/help/list").Include(
                      "~/Modules/BntWeb.HelpCenter/Content/Scripts/help.list.js"));
            bundles.Add(new ScriptBundle("~/js/admin/help/edit").Include(
                      "~/Modules/BntWeb.HelpCenter/Content/Scripts/help.edit.js"));

            
            bundles.Add(new ScriptBundle("~/js/admin/helpcenter/categorylist").Include(
                      "~/Modules/BntWeb.HelpCenter/Content/Scripts/helpcenter.category.list.js"));
            bundles.Add(new ScriptBundle("~/js/admin/helpcenter/categoryedit").Include(
                      "~/Modules/BntWeb.HelpCenter/Content/Scripts/helpcenter.category.edit.js"));

            //css
            bundles.Add(new StyleBundle("~/css/helpcenter").Include(
                "~/Resources/Web/Css/reset.css",
                "~/Resources/Web/Css/style.css",
                "~/Resources/Web/swiper/swiper.css"));

            bundles.Add(new ScriptBundle("~/js/helpcenter").Include(
                      "~/Resources/Web/Scripts/jQuery.js",
                      "~/Resources/Web/Scripts/jquery.easing.1.3.js",
                      "~/Resources/Web/Scripts/jquery.transit.js",
                      "~/Resources/Web/Scripts/prefixfree.min.js",
                      "~/Resources/Web/Scripts/bocfe.js",
                      "~/Resources/Web/swiper/swiper.min.js",
                      "~/Resources/Web/Scripts/mall.js"));
        }
    }
}