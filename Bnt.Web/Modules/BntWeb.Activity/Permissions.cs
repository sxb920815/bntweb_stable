/* 
    ======================================================================== 
        File name：        Permissions
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/6/16 11:55:21
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Security.Permissions;

namespace BntWeb.Activity
{
    public class Permissions : IPermissionProvider
    {
        private static readonly string CategoryKey = ActivityModule.DisplayName;

        public const string ViewActivityKey = "BntWeb-Activity-ViewActivity";
        public static readonly Permission ViewActivity = new Permission { Description = "查看活动", Name = ViewActivityKey, Category = CategoryKey };


        public const string DeleteActivityKey = "BntWeb-Activity-DeleteActivity";
        public static readonly Permission DeleteActivity = new Permission { Description = "删除活动", Name = DeleteActivityKey, Category = CategoryKey };

        public const string EditActivityKey = "BntWeb-Activity-EditActivity";
        public static readonly Permission EditActivity = new Permission { Description = "编辑活动", Name = EditActivityKey, Category = CategoryKey };


        public const string ViewTypeKey = "BntWeb-Activity-ViewType";
        public static readonly Permission ViewType = new Permission { Description = "查看类型", Name = ViewTypeKey, Category = CategoryKey };

        public const string DeleteTypeKey = "BntWeb-Activity-DeleteType";
        public static readonly Permission DeleteType = new Permission { Description = "删除类型", Name = DeleteTypeKey, Category = CategoryKey };

        public const string EditTypeKey = "BntWeb-Activity-EditType";
        public static readonly Permission EditType = new Permission { Description = "编辑类型", Name = EditTypeKey, Category = CategoryKey };

        public const string ManageCommentKey = "BntWeb-Activity-ManageComment";
        public static readonly Permission ManageComment = new Permission { Description = "管理评论", Name = ManageCommentKey, Category = CategoryKey };

        public const string ManageTagKey = "BntWeb-Activity-ManageTag";
        public static readonly Permission ManageTag = new Permission { Description = "管理标签", Name = ManageTagKey, Category = CategoryKey };


        public const string ViewApplyKey = "BntWeb-Activity-ViewApply";
        public static readonly Permission ViewApply = new Permission { Description = "查看报名人员", Name = ViewApplyKey, Category = CategoryKey };

        public int Position => ActivityModule.Position;

        public string Category => CategoryKey;

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ViewActivity,
                EditActivity,
                DeleteActivity,
                ManageComment,
                ViewType,
                EditType,
                DeleteType,
                ManageTag,
                ViewApply
            };
        }
    }
}