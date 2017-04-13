/* 
    ======================================================================== 
        File name：		BundleProvider
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 10:40:30
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System.Web.Optimization;
using BntWeb.UI.Bundle;

namespace BntWeb.Merchant
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //Js
            bundles.Add(new ScriptBundle("~/js/admin/merchant/type").Include(
                      "~/Modules/BntWeb.Merchant/Content/Scripts/merchant.type.js"));
            bundles.Add(new ScriptBundle("~/js/admin/merchant/list").Include(
                      "~/Modules/BntWeb.Merchant/Content/Scripts/merchant.list.js"));
            bundles.Add(new ScriptBundle("~/js/admin/merchant/edit").Include(
                      "~/Modules/BntWeb.Merchant/Content/Scripts/merchant.edit.js"));
            bundles.Add(new ScriptBundle("~/js/admin/merchant/productlist").Include(
                      "~/Modules/BntWeb.Merchant/Content/Scripts/merchant.products.js"));

            bundles.Add(new ScriptBundle("~/web/html/merchant/product/js").Include(
                      "~/Resources/Web/Scripts/jQuery.js"
                      , "~/Resources/Web/Scripts/jquery.easing.1.3.js"
                      , "~/Resources/Web/Scripts/jquery.transit.js"
                      , "~/Resources/Web/Scripts/prefixfree.min.js"
                      , "~/Resources/Web/Scripts/bocfe.js"
                      , "~/Resources/Web/swiper/swiper.min.js"));

            //css
            bundles.Add(new StyleBundle("~/web/merchant/css").Include(
                "~/Resources/Web/Css/reset.css",
                "~/Resources/Web/Css/style.css",
                "~/Resources/Web/swiper/swiper.css"));
        }
    }
}