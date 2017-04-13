using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BntWeb.Merchant.ApiModels;
using BntWeb.Merchant.Services;
using BntWeb.Utility.Extensions;
using BntWeb.WebApi.Models;

namespace BntWeb.Merchant.ApiControllers
{
    public class MerchantAcivityController : BaseApiController
    {
        private readonly IMerchantServices _merchantServices;
        public MerchantAcivityController(IMerchantServices merchantServices)
        {
            _merchantServices = merchantServices;
        }


        /// <summary>
        /// 获取商家列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult SearchAll(string keyword, int pageNo = 1, int limit = 10)
        {
            var totalCount = 0;
            List<Guid> typeList = new List<Guid>();

            var list = _merchantServices.GetAllSearchOnPage(pageNo, limit, keyword, typeList, out totalCount);

            var result = new ApiResult();
            var data = new
            {
                List = list.Select(me => new SearchAllModel(me)).ToList()
            };

            result.SetData(data);
            return result;
        }
    }
}
