/* 
    ======================================================================== 
        File name：		IMerchantServices
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/24 11:41:03
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BntWeb.FileSystems.Media;
using BntWeb.Merchant.Models;

namespace BntWeb.Merchant.Services
{
    public interface IMerchantServices : IDependency
    {
        #region 商家
        /// <summary>
        /// 分页获取商家雷彪
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="expression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isDesc"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.Merchant> GetListPaged<TKey>(int pageIndex, int pageSize,
            Expression<Func<Models.Merchant, bool>> expression,
            Expression<Func<Models.Merchant, TKey>> orderByExpression, bool isDesc, out int totalCount);
        /// <summary>
        /// 按类型和关键字获取商家信息
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="typeList"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.Merchant> GetListPagedByType(int pageIndex, int pageSize,
            string keyword, List<Guid> typeList, out int totalCount);
        Models.Merchant GetMerchantById(Guid id);
        bool Delete(Models.Merchant merchant);
        Guid UpdateMerchant(Models.Merchant merchant);
        Guid CreateMerchant(Models.Merchant merchant);
        /// <summary>
        /// 获取商家图片
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        StorageFile GetMerchantFile(Guid merchantId, string imageType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.Merchant> Search(string keyword, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 搜索接口，获取全部搜索结果：活动和商家
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="typeList"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<MerchantActivity> GetAllSearchOnPage(int pageIndex, int pageSize,
            string keyword, List<Guid> typeList, out int totalCount);

        #endregion
    }
}