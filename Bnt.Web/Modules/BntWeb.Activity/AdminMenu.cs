/* 
    ======================================================================== 
        File name：        AdminMenu
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/6/16 11:52:13
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Tag;
using BntWeb.UI.Navigation;

namespace BntWeb.Activity
{
    public class AdminMenu : INavigationProvider
    {
        //public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(ActivityModule.Key, ActivityModule.DisplayName, ActivityModule.Position.ToString(), BuildMenu, new List<string> { "icon-beer" });
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(ActivityModule.Key + "-TypesList", "活动类别", "10",
                item => item
                    .Action("TypeList", "Admin", new { area = ActivityModule.Area })
                    .Permission(Permissions.ViewActivity)
                );

            menu.Add(ActivityModule.Key + "-ActivitysList", "活动列表", "20",
                item => item
                    .Action("List", "Admin", new { area = ActivityModule.Area })
                    .Permission(Permissions.ViewActivity)
                );

            menu.Add(ActivityModule.Key + "-TagsList", "活动标签", "30",
                item => item
                    .Action("Index", "Admin", new { area = TagModule.Area, sourceType = "Activitys", moduleKey = ActivityModule.Key })
                    .Permission(Permissions.ManageTag)
                );

        }
    }
}