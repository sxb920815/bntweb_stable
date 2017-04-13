/* 
    ======================================================================== 
        File name：		AdminMenu
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 10:35:29
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

namespace BntWeb.Topic
{
    public class AdminMenu : INavigationProvider
    {
        //public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(TopicModule.Key, TopicModule.DisplayName, TopicModule.Position.ToString(), BuildMenu, new List<string> { "icon-comments" });
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(TopicModule.Key + "-TopicsList", "话题列表", "10",
                item => item
                    .Action("List", "Admin", new { area = TopicModule.Area })
                    .Permission(Permissions.ViewTopic)
                );

        }
    }
}