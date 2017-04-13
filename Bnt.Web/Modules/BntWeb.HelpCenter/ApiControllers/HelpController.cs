/* 
    ======================================================================== 
        File name：		Help
        Module:			
        Author：		Kahr.Lu（陆康康）
        Create Time：		2016/7/6 19:29:21
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Web.Http;
using BntWeb.HelpCenter.Services;
using BntWeb.FileSystems.Media;
using BntWeb.HelpCenter.ApiModels;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.WebApi.Filters;
using BntWeb.HelpCenter.Models;
using BntWeb.WebApi.Models;

namespace BntWeb.HelpCenter.ApiControllers
{
    public class HelpController : BaseApiController
    {
        private readonly IHelpCenterService _helpCenterService;

        public HelpController(IHelpCenterService helpCenterService)
        {
            _helpCenterService = helpCenterService;
        }

        /// <summary>
        /// 获取帮助列表
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult List(Guid categoryId, int pageNo = 1, int limit = 5)
        {
            var categoryModel = _helpCenterService.GetCategoryById(categoryId);
            if (categoryModel == null)
                throw new WebApiInnerException("1012", "类别不存在");

            int totalCount;
            var list = _helpCenterService.GetListPagedByCategory<Help>(categoryId, pageNo, limit, out totalCount);
            
            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Helps = list.Select(item => new ListHelpModel(item)).ToList()
            };
            result.SetData(data);

            return result;
        }

        /// <summary>
        /// 获取帮助详情
        /// </summary>
        /// <param name="helpId"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult HelpDetail(string helpId)
        {
            Argument.ThrowIfNullOrEmpty(helpId, "帮助Id");
            var model = _helpCenterService.GetHelpById(helpId.ToGuid());
            if (model == null)
                throw new WebApiInnerException("1021", "帮助不存在");

            var result = new ApiResult();
            var data = new DetaliHelpModel(model);
            result.SetData(data);
            return result;
        }
    }
}
