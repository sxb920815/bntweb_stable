/* 
    ======================================================================== 
        File name：		MerchantProductServices
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/28 17:22:36
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BntWeb.FileSystems.Media;
using BntWeb.Merchant.Models;
using BntWeb.Data;
using BntWeb.Logging;
using BntWeb.Data.Services;
using BntWeb.Security;
using BntWeb.Merchant.ViewModels;

namespace BntWeb.Merchant.Services
{
    public class MerchantProductServices : IMerchantProductServices
    {
        #region private定义
        private readonly ICurrencyService _currencyService;
        private readonly IStorageFileService _storageFileService;
        public ILogger Logger { get; set; }
        #endregion

        #region 构造
        public MerchantProductServices(ICurrencyService currencyService, IStorageFileService storageFileService)
        {
            _currencyService = currencyService;
            _storageFileService = storageFileService;
        }
        #endregion

        public Guid Create(MerchantProduct model)
        {
            model.Id = KeyGenerator.GetGuidKey();
            model.CreateTime = DateTime.Now;
            model.Status = MerchantProductStatus.Normal;            

            var result = _currencyService.Create<Models.MerchantProduct>(model);
            if (!result)
            {
                return Guid.Empty;
            }
            Logger.Operation($"创建优惠信息-{model.ProductName}:{model.Id}", MerchantModule.Instance, SecurityLevel.Normal);
            return model.Id;
        }

        public bool Delete(MerchantProduct model)
        {
            var result = _currencyService.DeleteByConditon<Models.MerchantProduct>(
                me => me.Id == model.Id);
            if (result>0)
                Logger.Operation($"删除优惠信息-{model.ProductName}:{model.Id}", MerchantModule.Instance, SecurityLevel.Warning);

            return result > 0;
        }

        public List<MerchantProduct> GetListPaged<TKey>(int pageIndex, int pageSize,string merchantName,string productName,
            Expression<Func<MerchantProduct, TKey>> orderByExpression, bool isDesc, out int totalCount)
        {
            using (var dbContext = new MerchantDbContext())
            {
                var query = dbContext.MerchantProducts.Include(m=>m.Merchant).Where(mp => mp.Status > 0);
                if (productName != "")
                    query = query.Where(mp => mp.ProductName.Contains(productName));
                if (merchantName != "")
                    query = query.Where(mp => mp.Merchant.MerchantName.Contains(merchantName));

                //query.Include(m => m.Merchant);
                totalCount = query.Count();
                if (isDesc)
                    query = query.OrderByDescending(orderByExpression);
                else
                    query = query.OrderBy(orderByExpression);

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        /// <summary>
        /// 获取商家的优惠信息列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="merchantId"></param>
        /// <param name="productName"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isDesc"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<MerchantProduct> GetListPaged<TKey>(int pageIndex, int pageSize, Expression<Func<Models.MerchantProduct, bool>> expression,
           Expression<Func<MerchantProduct, TKey>> orderByExpression, bool isDesc, out int totalCount)
        {
            using (var dbContext = new MerchantDbContext())
            {
                var query = dbContext.MerchantProducts.Include(m => m.Merchant).Where(mp => mp.Status > 0).Where(expression);                
                
                totalCount = query.Count();
                if (isDesc)
                    query = query.OrderByDescending(orderByExpression);
                else
                    query = query.OrderBy(orderByExpression);

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        public MerchantProduct GetMerchantProductById(Guid id)
        {
            return _currencyService.GetSingleById<Models.MerchantProduct>(id);
        }

        public IList<StorageFile> GetMerchantProductFile(Guid productId, string imageType)
        {
            return _storageFileService.GetFiles(productId, MerchantModule.Key, imageType);
        }
        
        //获取商家优惠信息列表
        public List<MerchantProduct> GetMerchantProductsByMId(Guid merchantiId, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new MerchantDbContext())
            {
                var query = dbContext.MerchantProducts.Where(me=>me.MerchantId == merchantiId && me.Status>0).OrderByDescending(me=>me.CreateTime);
               
                totalCount = query.Count();              

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        public Guid Update(MerchantProduct model)
        {
            var result = _currencyService.Update<Models.MerchantProduct>(model);
            if (!result)
                return Guid.Empty;
            Logger.Operation($"更新商家优惠信息-{model.ProductName}:{model.Id}", MerchantModule.Instance, SecurityLevel.Normal);
            return model.Id;
        }

        public List<MerchantSelect> GetMerchantSelectList()
        {
            using (var dbContext = new MerchantDbContext())
            {
                var query = from m in dbContext.Merchants
                            where m.Status > 0
                            orderby m.MerchantName ascending
                            select new MerchantSelect { MerchantId = m.Id, MerchantName=m.MerchantName };
                

                var list = query.ToList<MerchantSelect>();
                return list;
            }
        }
    }
}