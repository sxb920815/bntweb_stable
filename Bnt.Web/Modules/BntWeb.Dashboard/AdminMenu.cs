/* 
    ======================================================================== 
        File name：        AdminMenu
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/5/9 11:41:19
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Security;
using BntWeb.UI.Navigation;

namespace BntWeb.Dashboard
{
    public class AdminMenu : INavigationProvider
    {
        //public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.AddImageSet("dashboard")
                .Add("", "控制台", "-9999",
                    menu => menu.Add("BntWeb-Dashboard", "控制台", "1",
                        item => item
                            .Action("Index", "Admin", new { area = "Dashboard" })
                            //.Permission(StandardPermissions.AccessAdminPanel)
                            ), new List<string> { "icon-dashboard" });
        }
    }
}