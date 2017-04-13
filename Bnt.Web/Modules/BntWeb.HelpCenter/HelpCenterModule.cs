/* 
    ======================================================================== 
        File name：        ActivityModule
        Module:                
        Author：            Kahr.Lu(陆康康)
        Create Time：    2016/6/30 14:55:31
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Environment;

namespace BntWeb.HelpCenter
{
    public class HelpCenterModule:IBntWebModule
    {
        public static readonly HelpCenterModule Instance = new HelpCenterModule();

        public string InnerKey => "BntWeb-HelpCenter";
        public static string Key => Instance.InnerKey;
        public string InnerDisplayName => "帮助中心";
        public static string DisplayName => Instance.InnerDisplayName;
        public string InnerArea => "HelpCenter";
        public static string Area => Instance.InnerArea;
        public int InnerPosition => 9000;
        public static int Position => Instance.InnerPosition;
    }
}