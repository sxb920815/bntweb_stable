using BntWeb.Merchant.ApiModels;
using BntWeb.Merchant.Services;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BntWeb.Merchant.ApiControllers
{
    public class MerchantTypeController : BaseApiController
    {

        private readonly IMerchantTypeServices _merchantTypeServices;
        public MerchantTypeController(IMerchantTypeServices merchantTypeServices)
        {
            _merchantTypeServices = merchantTypeServices;
        }

        /// <summary>
        /// 递归商家分类树结构
        /// </summary>
        /// <param name="toType"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        private List<MerchantTypeModel> setTypeTree(MerchantTypeModel toType, List<Models.MerchantType> types)
        {
            var childTypes = types.Where(me => me.ParentId == toType.Id).OrderByDescending(me => me.Sort);
            if (childTypes == null)
                return new List<MerchantTypeModel>();
            else
            {
                toType.ChildMerchantTypes = new List<MerchantTypeModel>();
                foreach (var child in childTypes)
                {
                    var item = new MerchantTypeModel();
                    item.Id = child.Id;
                    item.TypeName = child.TypeName;
                    item.MerchantsCount = _merchantTypeServices.HasMerchantCount(item.Id);
                    item.ChildMerchantTypes = setTypeTree(item, types);
                    toType.ChildMerchantTypes.Add(item);
                }
                return toType.ChildMerchantTypes;
            }
        }

        /// <summary>
        /// 获取商家分类树结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetMerchantTypesTree()
        {
            var list = _merchantTypeServices.GetTypes();

            List<MerchantTypeModel> typeList = new List<MerchantTypeModel>();

            foreach (var item in list.Where(me => me.ParentId == Guid.Empty).OrderByDescending(me => me.Sort))
            {
                var type = new MerchantTypeModel();
                type.Id = item.Id;
                type.TypeName = item.TypeName;
                type.MerchantsCount = _merchantTypeServices.HasMerchantCount(type.Id);
                type.ChildMerchantTypes = setTypeTree(type, list);
                typeList.Add(type);
            }

            var result = new ApiResult();
            var data = new
            {
                MerchantTypes = typeList
            };

            result.SetData(data);

            return result;
        }

    }
}