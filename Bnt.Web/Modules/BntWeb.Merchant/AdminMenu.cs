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
using BntWeb.UI.Navigation;

namespace BntWeb.Merchant
{
    public class AdminMenu : INavigationProvider
    {
        //public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(MerchantModule.Key, MerchantModule.DisplayName, MerchantModule.Position.ToString(), BuildMenu, new List<string> { "icon-food" });
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(MerchantModule.Key + "-TypesList", "商家分类", "10",
                item => item
                    .Action("TypeList", "Admin", new { area = MerchantModule.Area })
                    .Permission(Permissions.ViewMerchantType)
                );
            menu.Add(MerchantModule.Key + "-MerchantsList", "商家列表", "20",
                item => item
                    .Action("List", "Merchant", new { area = MerchantModule.Area })
                    .Permission(Permissions.ViewMerchant)
                );

        }
    }
}