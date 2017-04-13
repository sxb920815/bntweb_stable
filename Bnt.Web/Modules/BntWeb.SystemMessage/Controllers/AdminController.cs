using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;

using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BntWeb.Logging;
using BntWeb.SystemMessage.Services;
using BntWeb.SystemMessage.ViewModels;

namespace BntWeb.SystemMessage.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly ISystemMessageService _systemMessageService;

        public AdminController(ICurrencyService currencyService, ISystemMessageService systemMessageService)
        {
            _currencyService = currencyService;
            _systemMessageService = systemMessageService;

            Logger = NullLogger.Instance;
        }
        public ILogger Logger { get; set; }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewSystemMessageKey })]
        public ActionResult List()
        {
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewSystemMessageKey })]
        public ActionResult ListOnPage()
        {
            var result = new DataTableJsonResult();
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;

            var list = _systemMessageService.GetSystemMessageListByPage(pageIndex, pageSize, out totalCount);
            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditSystemMessageKey })]
        public ActionResult CreateSystemMessage()
        {
            var systemMessage = new Models.SystemMessage { Id = Guid.Empty };
            return View("Edit", systemMessage);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditSystemMessageKey })]
        public ActionResult EditOnPost(SystemMessageViewModel systemMessage)
        {
            var result = new DataJsonResult();

            if (!_systemMessageService.CreatePushSystemMessage(systemMessage.Title, systemMessage.Content, null, null, null, null, "System", SystemMessageModule.Key))
            {
                result.ErrorMessage = "保存失败";
            }

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.DeleteSystemMessageKey })]
        public ActionResult Delete(Guid systemMessageId)
        {
            var result = new DataJsonResult();
            _currencyService.DeleteByConditon<Models.SystemMessage>(s => s.Id.Equals(systemMessageId));
            _currencyService.DeleteByConditon<Models.SystemMessageReciever>(s => s.MessageId.Equals(systemMessageId));
            return Json(result);
        }

    }
}