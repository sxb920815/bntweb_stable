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
using BntWeb.UI.Navigation;

namespace BntWeb.PromotionChannel
{
    public class AdminMenu : INavigationProvider
    {
        //public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(PromotionChannelModule.Key, PromotionChannelModule.DisplayName, PromotionChannelModule.Position.ToString(), BuildMenu, new List<string> { "icon-sitemap" });
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(PromotionChannelModule.Key + "-ChannelsList", "渠道列表", "20",
                item => item
                    .Action("List", "Admin", new { area = PromotionChannelModule.Area })
                    .Permission(Permissions.ManageChannel)
                );

        }

    }
}