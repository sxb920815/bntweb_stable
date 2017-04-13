/* 
    ======================================================================== 
        File name：		TopicModule
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 10:30:49
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Environment;

namespace BntWeb.Topic
{
    public class TopicModule : IBntWebModule
    {
        public static readonly TopicModule Instance = new TopicModule();

        public string InnerKey => "BntWeb-Topic";
        public static string Key => Instance.InnerKey;
        public string InnerDisplayName => "话题管理";
        public static string DisplayName => Instance.InnerDisplayName;
        public string InnerArea => "Topic";
        public static string Area => Instance.InnerArea;
        public int InnerPosition => 8000;
        public static int Position => Instance.InnerPosition;
    }
}