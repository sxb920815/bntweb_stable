using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.UI.Bundle;
using System.Web.Optimization;

namespace BntWeb.Article
{
    public class BundleProvider: IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //Js
            bundles.Add(new ScriptBundle("~/js/admin/article/list").Include(
                      "~/Modules/BntWeb.Article/Content/Scripts/article.list.js"));

            bundles.Add(new ScriptBundle("~/js/admin/Categories/index").Include(
                      "~/Modules/BntWeb.Article/Content/Scripts/article.categories.js"));


            //css
            bundles.Add(new StyleBundle("~/css/article").Include(
                "~/Resources/Web/Css/reset.css",
                "~/Resources/Web/Css/style.css",
                "~/Resources/Web/swiper/swiper.css"));

            bundles.Add(new ScriptBundle("~/js/article").Include(
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