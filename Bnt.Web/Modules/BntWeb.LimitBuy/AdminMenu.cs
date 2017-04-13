using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Tag;
using BntWeb.UI.Navigation;

namespace BntWeb.LimitBuy
{
    public class AdminMenu : INavigationProvider
    {
        //public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(LimitBuyModule.Key, LimitBuyModule.DisplayName, LimitBuyModule.Position.ToString(), BuildMenu, new List<string> { "icon-shopping-cart" });
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(LimitBuyModule.Key + "-LimitSingleGoodsList", "产品列表", "10",
                item => item
                    .Action("List", "Admin", new { area = LimitBuyModule.Area })
                    .Permission(Permissions.LimitBuy)
                );
           
        }
    }
}