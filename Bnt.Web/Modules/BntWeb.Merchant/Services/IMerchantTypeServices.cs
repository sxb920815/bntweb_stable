using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BntWeb.Merchant.Models;

namespace BntWeb.Merchant.Services
{
    public interface IMerchantTypeServices:IDependency
    {
        #region 商家分类
        List<MerchantType> GetTypes();
        MerchantType GetTypeById(Guid id);
        MerchantType GetTypeByName(string typeName);
        Guid SaveType(MerchantType model);
        bool DeleteType(MerchantType model);

        bool InsetTypeRelation(MerchantTypeRalation model);

        bool DeleteRelations(Guid merchantId);

        List<MerchantTypeRalation> GetTypeRelations(Guid merchantId);

        int HasMerchantCount(Guid typeId);

        #endregion
    }
}
