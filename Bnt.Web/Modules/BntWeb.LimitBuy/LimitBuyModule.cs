using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Environment;

namespace BntWeb.LimitBuy
{
    public class LimitBuyModule :IBntWebModule
    { 
        public static readonly LimitBuyModule Instance=new LimitBuyModule();

        public string InnerKey => "BntWeb-LimitBuy";
        public static string Key => Instance.InnerKey;
        public string InnerDisplayName => "限时抢购";
        public static string DisplayName => Instance.InnerDisplayName;
        public string InnerArea => "LimitBuy";
        public static string Area => Instance.InnerArea;
        public int InnerPosition => 8100;
        public static int Position => Instance.InnerPosition;
    }
}