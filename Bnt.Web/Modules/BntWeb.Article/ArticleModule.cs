/* 
    ======================================================================== 
        File name：        ArticleModule
        Module:                
        Author：            WSQ
        Create Time：    2016/6/28  
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Environment;

namespace BntWeb.Article
{
    public class ArticleModule : IBntWebModule
    {

        public static readonly ArticleModule Instance = new ArticleModule();

        public string InnerKey => "BntWeb-Article";
        public static string Key => Instance.InnerKey;
        public string InnerDisplayName => "文章管理";
        public static string DisplayName => Instance.InnerDisplayName;
        public string InnerArea => "Article";
        public static string Area => Instance.InnerArea;
        public int InnerPosition => 7000;
        public static int Position => Instance.InnerPosition;
    }
}