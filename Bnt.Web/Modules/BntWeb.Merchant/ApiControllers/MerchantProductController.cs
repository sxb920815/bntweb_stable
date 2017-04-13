using BntWeb.Merchant.ApiModels;
using BntWeb.Merchant.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BntWeb.Merchant.Models;

namespace BntWeb.Merchant.ApiControllers
{
    public class MerchantProductController : BaseApiController
    {
        private readonly IMerchantProductServices _merchantProductServices;
        private readonly IMerchantServices _merchantServices;
        public MerchantProductController(IMerchantProductServices merchantProductServices,IMerchantServices merchantServices)
        {
            _merchantProductServices = merchantProductServices;
            _merchantServices = merchantServices;
        }
        
        /// <summary>
        /// 获取商家优惠信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetMerchantProducts(Guid merchantId, int pageNo = 1, int limit = 10)
        {
            var totalCount = 0;
            if (merchantId == Guid.Empty)
                throw new WebApiInnerException("2001", "Id异常");

            var list = _merchantProductServices.GetMerchantProductsByMId(merchantId,pageNo,limit,out totalCount);

            var result = new ApiResult();
            var data = new
            {
                Products = list.Select(me => new MerchantProductModel(me)).ToList()
            };
            result.SetData(data);

            return result;
        }
        
        /// <summary>
        /// 获取生活圈首页显示的商家信息
        /// type=1:首页，2推荐
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetHomeMerchant(int type, int pageNo = 1, int limit = 3)
        {
            var totalCount = 0;

            Expression<Func<Models.Merchant, bool>> expression;


            if (type == 1)
            {
                expression = m => (m.Status == MerchantStatus.Normal) &&
                     (m.IsHowInFront);
            }
            else
            {
                expression = m => (m.Status == MerchantStatus.Normal) &&
                     (m.IsRecommend);
            }

            Expression<Func<Models.Merchant, object>> orderByExpression = m => new { m.CreateTime };


            var list = _merchantServices.GetListPaged(pageNo, limit, expression, orderByExpression, true,
                out totalCount);

            var result = new ApiResult();
            var data = new
            {
                Products = list.Select(me => new HomeMerchantModel(me)).ToList()
            };
            result.SetData(data);

            return result;
        }
    }
}
