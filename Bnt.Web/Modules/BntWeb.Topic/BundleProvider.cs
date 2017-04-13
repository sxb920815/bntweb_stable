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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using BntWeb.UI.Bundle;

namespace BntWeb.Topic
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //Js
            bundles.Add(new ScriptBundle("~/js/admin/topic/list").Include(
                      "~/Modules/BntWeb.Topic/Content/Scripts/topic.list.js"));
        }
    }
}