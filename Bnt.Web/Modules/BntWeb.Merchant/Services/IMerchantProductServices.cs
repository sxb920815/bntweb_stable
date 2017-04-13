/* 
    ======================================================================== 
        File name：		IMerchantProductServices
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/28 17:22:02
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using BntWeb.FileSystems.Media;
using BntWeb.Merchant.Models;
using BntWeb.Merchant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BntWeb.Merchant.Services
{
    public interface IMerchantProductServices : IDependency
    {
        /// <summary>
        /// 分页获取商家优惠信息列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="expression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isDesc"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<MerchantProduct> GetListPaged<TKey>(int pageIndex, int pageSize, string merchantName, string productName,
            Expression<Func<MerchantProduct, TKey>> orderByExpression, bool isDesc, out int totalCount);

        List<MerchantProduct> GetListPaged<TKey>(int pageIndex, int pageSize, Expression<Func<Models.MerchantProduct, bool>> expression,
           Expression<Func<MerchantProduct, TKey>> orderByExpression, bool isDesc, out int totalCount);

        Models.MerchantProduct GetMerchantProductById(Guid id);
        bool Delete(Models.MerchantProduct model);
        Guid Update(Models.MerchantProduct model);
        Guid Create(Models.MerchantProduct model);
        /// <summary>
        /// 获取商家优惠图片
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        IList<StorageFile> GetMerchantProductFile(Guid productId, string imageType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantiId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.MerchantProduct> GetMerchantProductsByMId(Guid merchantiId, int pageIndex, int pageSize, out int totalCount);
        List<MerchantSelect> GetMerchantSelectList();
    }
}