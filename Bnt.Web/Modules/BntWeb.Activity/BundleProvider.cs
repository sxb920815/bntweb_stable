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

namespace BntWeb.Activity
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //Js
            bundles.Add(new ScriptBundle("~/js/admin/activitys/list").Include(
                      "~/Modules/BntWeb.Activity/Content/Scripts/activity.list.js"));
            bundles.Add(new ScriptBundle("~/js/admin/activitys/type").Include(
                      "~/Modules/BntWeb.Activity/Content/Scripts/activity.type.js"));
            bundles.Add(new ScriptBundle("~/js/admin/activitys/apply/list").Include(
                      "~/Modules/BntWeb.Activity/Content/Scripts/activity.apply.list.js"));
        }
    }
}