/* 
    ======================================================================== 
        File name：		MerchantServices
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/24 11:41:43
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Logging;
using BntWeb.Merchant.Models;
using BntWeb.Security;
using BntWeb.Core.SystemSettings.Models;

namespace BntWeb.Merchant.Services
{
    public class MerchantServices : IMerchantServices
    {
        #region private定义
        private readonly ICurrencyService _currencyService;
        private readonly IStorageFileService _storageFileService;
        public ILogger Logger { get; set; }
        #endregion

        #region 构造
        public MerchantServices(ICurrencyService currencyService, IStorageFileService storageFileService)
        {
            _currencyService = currencyService;
            _storageFileService = storageFileService;
        }
        #endregion

        public List<Models.Merchant> GetListPaged<TKey>(int pageIndex, int pageSize,
            Expression<Func<Models.Merchant, bool>> expression,
            Expression<Func<Models.Merchant, TKey>> orderByExpression,
            bool isDesc, out int totalCount)
        {
            using (var dbContext = new MerchantDbContext())
            {
                var query = dbContext.Merchants.Where(expression);
                totalCount = query.Count();
                if (isDesc)
                    query = query.OrderByDescending(orderByExpression);
                else
                    query = query.OrderBy(orderByExpression);

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }


        public List<Models.Merchant> GetListPagedByType(int pageIndex, int pageSize,
            string keyword, List<Guid> typeList, out int totalCount)
        {
            using (var dbContext = new MerchantDbContext())
            {
                var checkKeywork = string.IsNullOrWhiteSpace(keyword);
                IOrderedQueryable<Models.Merchant> query;
                if (typeList.Count > 0)
                {
                    query = from m in dbContext.Merchants
                            where (from mtr in dbContext.MerchantTypeRalations where typeList.Contains(mtr.MerchantTypeId) select mtr.MerchantId).Contains(m.Id) && m.Status>0
                            && (checkKeywork || m.MerchantName.Contains(keyword))
                            orderby m.MerchantName descending
                            select m;
                }
                else
                {
                    query = from m in dbContext.Merchants
                            where m.Status> 0 && (checkKeywork || m.MerchantName.Contains(keyword))
                            orderby m.MerchantName descending
                            select m;
                }
                //if(!string.IsNullOrWhiteSpace(keyword))
                //    query = query.Where(m => m.MerchantName.Contains(keyword));


                totalCount = query.Count();

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        public Models.Merchant GetMerchantById(Guid id)
        {
            return _currencyService.GetSingleById<Models.Merchant>(id);
        }

        public bool Delete(Models.Merchant model)
        {
            //删除类型关联
            var delCount = _currencyService.DeleteByConditon<Models.MerchantTypeRalation>(me=>me.MerchantId == model.Id);
            if (delCount > 0)
                Logger.Operation($"删除商家[{model.MerchantName}]的所有类型关联-", MerchantModule.Instance, SecurityLevel.Warning);
            //删除商家优惠信息
            delCount = _currencyService.DeleteByConditon<Models.MerchantProduct>(
                me => me.MerchantId == model.Id);
            if (delCount > 0)
                Logger.Operation($"删除商家[{model.MerchantName}]的所有优惠信息-", MerchantModule.Instance, SecurityLevel.Warning);

            //删除商家
            var bResult = _currencyService.Delete(model);
            if (bResult)
                Logger.Operation($"删除商家-{model.MerchantName}:{model.Id}", MerchantModule.Instance, SecurityLevel.Warning);

            return bResult;
        }

        public Guid UpdateMerchant(Models.Merchant model)
        {
            //var district = _currencyService.GetSingleById<District>(model.Street);
            //if (district != null)
            //    model.PCDS = district.MergerName;
            var result = _currencyService.Update<Models.Merchant>(model);
            if (!result)
                return Guid.Empty;
            Logger.Operation($"更新商家-{model.MerchantName}:{model.Id}", MerchantModule.Instance, SecurityLevel.Normal);
            return model.Id;
        }

        public Guid CreateMerchant(Models.Merchant model)
        {
            model.Id = KeyGenerator.GetGuidKey();
            model.CreateTime = DateTime.Now;
            model.Status = MerchantStatus.Normal;

            //var district = _currencyService.GetSingleById<District>(model.Street);
            //if (district != null)
            //    model.PCDS = district.MergerName;

            var result = _currencyService.Create<Models.Merchant>(model);
            if (!result)
            {
                return Guid.Empty;
            }
            Logger.Operation($"创建商家-{model.MerchantName}:{model.Id}", MerchantModule.Instance, SecurityLevel.Normal);
            return model.Id;
        }

        public StorageFile GetMerchantFile(Guid merchantId, string imageType)
        {
            return _storageFileService.GetFiles(merchantId, MerchantModule.Key, imageType).FirstOrDefault();
        }

        public List<Models.Merchant> Search(string keyword, int pageIndex, int pageSize, out int totalCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 搜索接口，获取全部搜索结果：活动和商家
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="typeList"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<MerchantActivity> GetAllSearchOnPage(int pageIndex, int pageSize,
            string keyword, List<Guid> typeList, out int totalCount)
        {
            using (var dbContext = new MerchantDbContext())
            {
                IQueryable<Models.MerchantActivity> query = dbContext.MerchantActivitys.Where(me=>me.Status>0);
                if (!string.IsNullOrWhiteSpace(keyword))
                    query = query.Where(me => me.Title.Contains(keyword));

                totalCount = query.Count();
                
                var data = query.OrderByDescending(me => me.CreateTime);

                return data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }
    }
}