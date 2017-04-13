/* 
    ======================================================================== 
        File name：		Permissions
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 10:37:10
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Security.Permissions;

namespace BntWeb.Topic
{
    public class Permissions : IPermissionProvider
    {
        private static readonly string CategoryKey = TopicModule.DisplayName;

        public const string ViewTopicKey = "BntWeb-Topic-ViewTopic";
        public static readonly Permission ViewTopic = new Permission { Description = "查看话题", Name = ViewTopicKey, Category = CategoryKey };


        public const string DeleteTopicKey = "BntWeb-Topic-DeleteTopic";
        public static readonly Permission DeleteTopic = new Permission { Description = "删除话题", Name = DeleteTopicKey, Category = CategoryKey };

        public const string ManageCommentKey = "BntWeb-Topic-ManageComment";
        public static readonly Permission ManageComment = new Permission { Description = "管理评论", Name = ManageCommentKey, Category = CategoryKey };

        public const string SetHotKey = "BntWeb-Topic-SetHot";
        public static readonly Permission SetHot = new Permission { Description = "设置热门", Name = SetHotKey, Category = CategoryKey };


        public int Position => TopicModule.Position;

        public string Category => CategoryKey;

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ViewTopic,
                DeleteTopic,
                ManageComment,
                SetHot
            };
        }
    }
}