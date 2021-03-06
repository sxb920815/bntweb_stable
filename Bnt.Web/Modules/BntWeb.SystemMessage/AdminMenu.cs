﻿/* 
    ======================================================================== 
        File name：        AdminMenu
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/5/9 11:41:19
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/

using BntWeb.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.SystemMessage;


namespace BntWeb.SystemMessage
{
    public class AdminMenu : INavigationProvider
    {
        //public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(SystemMessageModule.Key, SystemMessageModule.DisplayName, SystemMessageModule.Position.ToString(), BuildMenu, new List<string> { "icon-comment-alt" });
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(SystemMessageModule.Key + "-SystemMessageList", "系统消息", "20",
                item => item
                    .Action("List", "Admin", new { area = SystemMessageModule.Area })
                    .Permission(Permissions.ViewSystemMessage)
                );

        }

    }
}