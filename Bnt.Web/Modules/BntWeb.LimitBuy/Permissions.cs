
using System.Collections.Generic;
using BntWeb.Security.Permissions;

namespace BntWeb.LimitBuy
{
    public class Permissions : IPermissionProvider
    {
        private static readonly string CategoryKey = LimitBuyModule.DisplayName;

        public const string LimitBuyKey = "BntWeb-LimitBuy-LimitSingleGoodsList";
        public static readonly Permission LimitBuy = new Permission { Description = "秒杀商品管理", Name = LimitBuyKey, Category = CategoryKey };

        public int Position => LimitBuyModule.Position;

        public string Category => CategoryKey;

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                LimitBuy,
            };
        }
    }
}