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

namespace BntWeb.Merchant
{
    public class Permissions : IPermissionProvider
    {
        private static readonly string CategoryKey = MerchantModule.DisplayName;

        public const string ViewMerchantKey = "BntWeb-Merchant-ViewMerchant";
        public static readonly Permission ViewMerchant = new Permission { Description = "查看商家", Name = ViewMerchantKey, Category = CategoryKey };


        public const string DeleteMerchantKey = "BntWeb-Merchant-DeleteMerchant";
        public static readonly Permission DeleteMerchant = new Permission { Description = "删除商家", Name = DeleteMerchantKey, Category = CategoryKey };


        public const string EditMerchantKey = "BntWeb-Merchant-EditMerchant";
        public static readonly Permission EditMerchant = new Permission { Description = "编辑商家", Name = EditMerchantKey, Category = CategoryKey };

        public const string EditMerchantTypeKey = "BntWeb-Merchant-EditMerchantType";
        public static readonly Permission EditMerchantType = new Permission { Description = "编辑商家分类", Name = EditMerchantTypeKey, Category = CategoryKey };

        public const string ViewMerchantTypeKey = "BntWeb-Merchant-ViewMerchantType";
        public static readonly Permission ViewMerchantType = new Permission { Description = "查看商家", Name = ViewMerchantTypeKey, Category = CategoryKey };


        public const string ViewMerchantProductKey = "BntWeb-Merchant-ViewMerchantProduct";
        public static readonly Permission ViewMerchantProduct = new Permission { Description = "查看商家优惠", Name = ViewMerchantProductKey, Category = CategoryKey };


        public const string DeleteMerchantProductKey = "BntWeb-Merchant-DeleteMerchantProduct";
        public static readonly Permission DeleteMerchantProduct = new Permission { Description = "删除商家优惠", Name = DeleteMerchantProductKey, Category = CategoryKey };


        public const string EditMerchantProductKey = "BntWeb-Merchant-EditMerchantProduct";
        public static readonly Permission EditMerchantProduct = new Permission { Description = "编辑商家优惠", Name = EditMerchantProductKey, Category = CategoryKey };


        public int Position => MerchantModule.Position;

        public string Category => CategoryKey;

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ViewMerchant,
                DeleteMerchant,
                EditMerchant,
                EditMerchantType,
                ViewMerchantProduct,
                EditMerchantProduct,
                DeleteMerchantProduct
            };
        }
    }
}