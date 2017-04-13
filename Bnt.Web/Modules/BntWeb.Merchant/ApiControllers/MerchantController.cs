using BntWeb.Merchant.Services;
using BntWeb.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BntWeb.Utility.Extensions;
using BntWeb.Merchant.ApiModels;

namespace BntWeb.Merchant.ApiControllers
{
    public class MerchantController : BaseApiController
    {
        private readonly IMerchantServices _merchantServices;
        public MerchantController(IMerchantServices merchantServices)
        {
            _merchantServices = merchantServices;
        }

        /// <summary>
        /// 获取商家列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult SearchMerchant(string keyword, string typeIds, int pageNo = 1, int limit = 10)
        {
            var totalCount = 0;
            List<Guid> typeList = new List<Guid>();
            if (!string.IsNullOrWhiteSpace(typeIds))
                typeList = typeIds.Split(',').ToList().Select(me => me.ToGuid()).ToList();

            var list = _merchantServices.GetListPagedByType(pageNo, limit, keyword, typeList, out totalCount);

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Merchants = list.Select(me => new MerchantModel(me)).ToList()
            };

            result.SetData(data);
            return result;
        }


        /// <summary>
        /// 获取商家详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult Detail(Guid merchantId)
        {
            var detail = _merchantServices.GetMerchantById(merchantId);

            if (detail == null)
                throw new WebApiInnerException("1001", "商家不存在");

            var result = new ApiResult();
            result.SetData(new MerchantDetail(detail));
            return result;
        }

    }
}