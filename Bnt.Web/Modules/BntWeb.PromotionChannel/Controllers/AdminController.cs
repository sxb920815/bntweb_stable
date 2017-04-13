using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase.Models;
using BntWeb.Mvc;
using BntWeb.PromotionChannel.Models;
using BntWeb.Security;
using BntWeb.Security.Identity;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.Web.Extensions;

namespace BntWeb.PromotionChannel.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IStorageFileService _storageFileService;

        public AdminController(ICurrencyService currencyService, IStorageFileService storageFileService)
        {
            _currencyService = currencyService;
            _storageFileService = storageFileService;
        }


        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageChannelKey })]
        public ActionResult Create()
        {
            var channel = new Channel();
            return View("Edit", channel);
        }


        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageChannelKey })]
        public ActionResult Edit(Guid id)
        {
            var channel = _currencyService.GetSingleById<Channel>(id);
            return View(channel);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageChannelKey })]
        public ActionResult EditOnPost(Channel editChannel)
        {
            var result = new DataJsonResult();

            if (editChannel.Id == Guid.Empty)
            {
                editChannel.Id = KeyGenerator.GetGuidKey();
                editChannel.CreateTime = DateTime.Now;
                _currencyService.Create(editChannel);
            }
            else
            {
                var channel = _currencyService.GetSingleById<Channel>(editChannel.Id);
                if (channel != null)
                {
                    channel.Name = editChannel.Name;
                    _currencyService.Update(channel);
                }
            }
            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageChannelKey })]
        public ActionResult List()
        {
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageChannelKey })]
        public ActionResult ListOnPage()
        {
            var result = new DataTableJsonResult();

            //取参数值
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;

            Expression<Func<Channel, object>> orderByExpression;
            //设置排序
            switch (sortColumn)
            {
                case "CreateTime":
                    orderByExpression = c => new { c.CreateTime };
                    break;
                default:
                    orderByExpression = c => new { c.Name };
                    break;
            }

            //分页查询
            var channels = _currencyService.GetListPaged(pageIndex, pageSize, orderByExpression, isDesc, out totalCount);
            foreach (var channel in channels)
            {
                channel.UsersCount = _currencyService.Count<Member>(m => m.ChannelId != null && m.ChannelId == channel.Id);
            }
            result.data = channels;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageChannelKey })]
        public ActionResult Delete(Guid id)
        {
            var result = new DataJsonResult();
            _currencyService.DeleteByConditon<Channel>(c => c.Id == id);
            return Json(result);
        }
    }

}