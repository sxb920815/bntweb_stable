
using System.Web.Optimization;
using BntWeb.UI.Bundle;

namespace BntWeb.LimitBuy
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css/admin/bootstrap").Include(
                      "~/Modules/BntWeb.LimitBuy/Content/CSS/bootstrap.min.css"));
            //Js
            bundles.Add(new ScriptBundle("~/js/admin/limitbuy/product/list").Include(
                      "~/Modules/BntWeb.LimitBuy/Content/Scripts/product.list.js"));

            bundles.Add(new ScriptBundle("~/js/admin/product-edit").Include(
                      "~/Modules/BntWeb.LimitBuy/Content/Scripts/product.edit.js"));

            bundles.Add(new ScriptBundle("~/js/admin/bootstrap").Include(
                      "~/Modules/BntWeb.LimitBuy/Content/Scripts/bootstrap.min.js"));

        }
    }
}