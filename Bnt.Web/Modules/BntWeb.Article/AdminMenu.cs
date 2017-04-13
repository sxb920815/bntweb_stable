/* 
    ======================================================================== 
        File name：        AdminMenu
        Module:                
        Author：            WSQ
        Create Time：    2016/6/28  
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using BntWeb.UI.Navigation;

namespace BntWeb.Article
{
    public class AdminMenu: INavigationProvider
    {
         
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(ArticleModule.Key, ArticleModule.DisplayName, ArticleModule.Position.ToString(), BuildMenu, new List<string> { "icon-book" });
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(ArticleModule.Key + "-CategoriesList", "文章分类", "10",
                item => item
                    .Action("CategoriesList", "Admin", new { area = ArticleModule.Area })
                    .Permission(Permissions.ViewArticle)
                );

            menu.Add(ArticleModule.Key + "-List", "文章列表", "20",
                item => item
                    .Action("List", "Admin", new { area = ArticleModule.Area })
                    .Permission(Permissions.ViewArticle)
                ); 
        }
    }



}