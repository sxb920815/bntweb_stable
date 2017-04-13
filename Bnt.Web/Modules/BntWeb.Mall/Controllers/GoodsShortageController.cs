using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BntWeb.Config.Models;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Logging;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Services;
using BntWeb.Web.Extensions;

namespace BntWeb.Mall.Controllers
{
    public class GoodsShortageController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IConfigService _configService;

        public GoodsShortageController(ICurrencyService currencyService, IConfigService configService)
        {
            _currencyService = currencyService;
            _configService = configService;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        public ActionResult List()
        {
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        public ActionResult ListOnPage()
        {
            var result = new DataTableJsonResult();

            //取参数值
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;

            //取查询条件
            var name = Request.Get("extra_search[Name]");
            var checkName = string.IsNullOrWhiteSpace(name);

            var goodsNo = Request.Get("extra_search[GoodsNo]");
            var checkGoodsNo = string.IsNullOrWhiteSpace(goodsNo);

            var status = Request.Get("extra_search[Status]");
            var checkStatus = string.IsNullOrWhiteSpace(status);

            var config = _configService.Get<SystemConfig>();

            Expression<Func<Models.Goods, bool>> expression =
                l => (checkName || l.Name.ToString().Equals(name, StringComparison.OrdinalIgnoreCase)) &&
                     (checkGoodsNo || l.GoodsNo.Contains(goodsNo)) &&
                     (checkStatus || ((int)l.Status).ToString().Equals(status)) &&
                     l.Stock <= config.StockWarning &&
                     l.Status != GoodsStatus.Delete;

            //分页查询
            var list = _currencyService.GetListPaged<Goods>(pageIndex, pageSize, expression, out totalCount, new OrderModelField { PropertyName = sortColumn, IsDesc = isDesc });

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}