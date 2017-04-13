/* 
    ======================================================================== 
        File name：		MerchantTypeServices
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/24 11:43:05
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Logging;
using BntWeb.Merchant.Models;
using BntWeb.Security;

namespace BntWeb.Merchant.Services
{
    public class MerchantTypeServices : IMerchantTypeServices
    {
        #region private定义
        private readonly ICurrencyService _currencyService;

        public ILogger Logger { get; set; }
        #endregion

        #region 构造
        public MerchantTypeServices(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        #endregion

        #region 商家分类

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        public List<MerchantType> GetTypes()
        {
            using (var dbContext = new MerchantDbContext())
            {
                return dbContext.MerchantTypes.OrderByDescending(me=>me.Sort).ToList();
            }
        }

        public MerchantType GetTypeById(Guid id)
        {
            return _currencyService.GetSingleById<Models.MerchantType>(id);
        }
        public MerchantType GetTypeByName(string typeName)
        {
            return _currencyService.GetSingleByConditon<MerchantType>(me => me.TypeName == typeName);
        }
        /// <summary>
        /// 保存类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Guid SaveType(MerchantType model)
        {
            bool result;
            string operateType = "";
            model.MergerId = model.MergerId + "," + model.Id;
            model.MergerTypeName = model.MergerTypeName + "," + model.TypeName;
            if (model.Id == Guid.Empty)
            {
                operateType = "创建";
                model.Id = KeyGenerator.GetGuidKey();
                model.Status = 1;
                result = _currencyService.Create(model);
            }
            else
            {
                operateType = "更新";
                result = _currencyService.Update(model);
            }
            if (!result)
                return Guid.Empty;
            Logger.Operation($"{operateType}商家分类-{model.TypeName}:{model.Id}", MerchantModule.Instance, SecurityLevel.Normal);
            return model.Id;
        }
        /// <summary>
        /// 删除商家分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteType(MerchantType model)
        {
            var result = _currencyService.DeleteByConditon<MerchantType>(
                me => me.Id == model.Id);
            if (result > 0)
                Logger.Operation($"删除商家分类-{model.TypeName}:{model.Id}", MerchantModule.Instance, SecurityLevel.Warning);

            return result > 0;
        }

        public bool InsetTypeRelation(MerchantTypeRalation model)
        {
            model.Id = KeyGenerator.GetGuidKey();
            return _currencyService.Create(model);
        }

        public bool DeleteRelations(Guid merchantId)
        {
           return _currencyService.DeleteByConditon<MerchantTypeRalation>(me => me.MerchantId == merchantId)>0;
        }

        public List<MerchantTypeRalation> GetTypeRelations(Guid merchantId)
        {
            using( var dbContext = new MerchantDbContext())
            {
                return dbContext.MerchantTypeRalations.Include(m=>m.MerchantType).Where(me => me.MerchantId == merchantId).ToList();
            }
        }

        public int HasMerchantCount(Guid typeId)
        {
            using (var dbContext = new MerchantDbContext())
            {
                return dbContext.MerchantTypeRalations.Count(me=>me.MerchantTypeId == typeId);
            }
        }
        #endregion
    }
}