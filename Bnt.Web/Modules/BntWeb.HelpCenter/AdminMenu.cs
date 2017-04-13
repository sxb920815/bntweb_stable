/* 
    ======================================================================== 
        File name：        AdminMenu
        Module:                
        Author：            Kahr.Lu(陆康康)
        Create Time：    2016/6/30 15:00:00
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.UI.Navigation;

namespace BntWeb.HelpCenter
{
    public class AdminMenu: INavigationProvider
    {
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(HelpCenterModule.Key, HelpCenterModule.DisplayName, HelpCenterModule.Position.ToString(), BuildMenu, new List<string> { "icon-beer" });
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(HelpCenterModule.Key + "-HelpCategoriesList", "帮助类别", "10",
                item => item
                    .Action("CategoryList", "Admin", new { area = HelpCenterModule.Area })
                    .Permission(Permissions.ViewHelpCategory)
                );

            menu.Add(HelpCenterModule.Key + "-HelpsList", "帮助列表", "20",
                item => item
                    .Action("List", "Admin", new { area = HelpCenterModule.Area })
                    .Permission(Permissions.ViewHelp)
                );

        }
    }
}