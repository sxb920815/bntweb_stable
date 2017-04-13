/* 
    ======================================================================== 
        File name：        Permissions
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/5/9 15:34:55
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/

using System.Collections.Generic;
using BntWeb.Security.Permissions;

namespace BntWeb.PromotionChannel
{
    public class Permissions : IPermissionProvider
    {
        private static readonly string CategoryKey = PromotionChannelModule.DisplayName;

        public const string ManageChannelKey = "BntWeb-MemberCenter-ManageChannel";
        public static readonly Permission ManageChannel = new Permission { Description = "渠道管理", Name = ManageChannelKey, Category = CategoryKey };


        public int Position => PromotionChannelModule.Position;

        public string Category => CategoryKey;

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageChannel
            };
        }
    }
}