/* 
    ======================================================================== 
        File name：        Permissions
        Module:                
        Author：            Kahr.Lu(陆康康)
        Create Time：    2016/6/30 15:03:21
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Security.Permissions;

namespace BntWeb.HelpCenter
{
    public class Permissions:IPermissionProvider
    {
        private static readonly string CategoryKey = HelpCenterModule.DisplayName;

        public const string ViewHelpKey = "BntWeb-Help-ViewHelp";
        public static readonly Permission ViewHelp = new Permission { Description = "查看问题", Name = ViewHelpKey, Category = CategoryKey };


        public const string DeleteHelpKey = "BntWeb-Help-DeleteHelp";
        public static readonly Permission DeleteHelp = new Permission { Description = "删除问题", Name = DeleteHelpKey, Category = CategoryKey };

        public const string EditHelpKey = "BntWeb-Help-EditHelp";
        public static readonly Permission EditHelp = new Permission { Description = "编辑问题", Name = EditHelpKey, Category = CategoryKey };


        public const string ViewHelpCategoryKey = "BntWeb-HelpCategory-ViewHelpCategory";
        public static readonly Permission ViewHelpCategory = new Permission { Description = "查看问题类别", Name = ViewHelpCategoryKey, Category = CategoryKey };

        public const string DeleteHelpCategoryKey = "BntWeb-HelpCategory-DeleteHelpCategory";
        public static readonly Permission DeleteHelpCategory = new Permission { Description = "删除问题类别", Name = DeleteHelpCategoryKey, Category = CategoryKey };

        public const string EditHelpCategoryKey = "BntWeb-HelpCategory-EditHelpCategory";
        public static readonly Permission EditHelpCategory = new Permission { Description = "编辑问题类别", Name = EditHelpCategoryKey, Category = CategoryKey };

        public int Position => HelpCenterModule.Position;

        public string Category => CategoryKey;

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ViewHelp,
                DeleteHelp,
                EditHelp,
                ViewHelpCategory,
                DeleteHelpCategory,
                EditHelpCategory
            };
        }
    }
}