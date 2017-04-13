/* 
    ======================================================================== 
        File name：        MemberCenterModule
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/6/7 15:46:55
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Environment;

namespace BntWeb.PromotionChannel
{
    public class PromotionChannelModule : IBntWebModule
    {

        public static readonly PromotionChannelModule Instance = new PromotionChannelModule();

        public string InnerKey => "BntWeb-PromotionChannel";
        public static string Key => Instance.InnerKey;
        public string InnerDisplayName => "推广渠道";
        public static string DisplayName => Instance.InnerDisplayName;
        public string InnerArea => "PromotionChannel";
        public static string Area => Instance.InnerArea;
        public int InnerPosition => 8000;
        public static int Position => Instance.InnerPosition;
    }
}