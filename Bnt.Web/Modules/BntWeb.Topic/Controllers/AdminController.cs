/* 
    ======================================================================== 
        File name：		TopicController
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 16:19:58
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Topic.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Web.Extensions;
using LinqKit;

namespace BntWeb.Topic.Controllers
{
    public class AdminController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly IStorageFileService _storageFileService;
        private readonly ICurrencyService _currencyService;
        private readonly IUserContainer _userContainer;

        /// <summary>
        /// 定义文件类型
        /// </summary>
        private const string FileTypeName = "TopicImage";

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="topicService"></param>
        /// <param name="storageFileService"></param>
        /// <param name="currencyService"></param>
        /// <param name="userContainer"></param>
        public AdminController(ITopicService topicService, IStorageFileService storageFileService, ICurrencyService currencyService, IUserContainer userContainer)
        {
            _topicService = topicService;
            _storageFileService = storageFileService;
            _currencyService = currencyService;
            _userContainer = userContainer;
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewTopicKey })]
        public ActionResult List()
        {
            var types = _topicService.GetTypes();
            ViewBag.Types = types;
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewTopicKey })]
        public ActionResult ListOnPage()
        {
            var result = new DataTableJsonResult();

            //取参数值
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc = true;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;

            //取查询条件
            var typeId = Request.Get("extra_search[TypeId]");
            var checkTypeId = string.IsNullOrWhiteSpace(typeId);

            var isHot = Request.Get("extra_search[IsHot]");
            var checkIsHot = string.IsNullOrWhiteSpace(isHot);
            bool bHot = false;
            if (!checkIsHot)
                bHot = Convert.ToBoolean(Convert.ToInt32(isHot));

            var topicContent = Request.Get("extra_search[TopicContent]");
            var checkContent = string.IsNullOrWhiteSpace(topicContent);

            var memberName = Request.Get("extra_search[MemberName]");
            var checkMemberName = string.IsNullOrWhiteSpace(memberName);

            var createTimeBegin = Request.Get("extra_search[CreateTimeBegin]");
            var checkCreateTimeBegin = string.IsNullOrWhiteSpace(createTimeBegin);
            var createTimeBeginTime = createTimeBegin.To<DateTime>();

            var createTimeEnd = Request.Get("extra_search[CreateTimeEnd]");
            var checkCreateTimeEnd = string.IsNullOrWhiteSpace(createTimeEnd);
            var createTimeEndTime = createTimeEnd.To<DateTime>();

            Expression<Func<Models.Topic, bool>> expression =
                l => (checkTypeId || l.TypeId.ToString().Equals(typeId, StringComparison.OrdinalIgnoreCase)) &&
                     (checkContent || l.TopicContent.Contains(topicContent)) &&
                     (checkMemberName || l.MemberName.Contains(memberName)) &&
                     (checkCreateTimeBegin || l.CreateTime >= createTimeBeginTime) &&
                     (checkCreateTimeEnd || l.CreateTime <= createTimeEndTime) &&
                     (checkIsHot || l.IsHot == bHot) &&
                     (l.Status > 0);
            
            Expression<Func<Models.Topic, object>> orderByExpression;
            //设置排序
            switch (sortColumn)
            {
                case "CreateTime":
                    orderByExpression = act => new { act.CreateTime };
                    break;
                default:
                    orderByExpression = act => new { act.CreateTime };
                    break;
            }

            //分页查询
            var list = _topicService.GetListPaged(pageIndex, pageSize, expression, orderByExpression,
                isDesc, out totalCount);

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.DeleteTopicKey })]
        public ActionResult Delete(Guid topicId)
        {
            var result = new DataJsonResult();
            Models.Topic topic = _currencyService.GetSingleById<Models.Topic>(topicId);

            if (topic != null)
            {

                var isDelete = _topicService.Delete(topic);

                if (!isDelete)
                {
                    result.ErrorMessage = "删除失败!";
                }
            }
            else
            {
                result.ErrorMessage = "话题不存在！";
            }

            return Json(result);
        }


        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewTopicKey })]
        public ActionResult Detail(Guid topicId)
        {
            var  model = _topicService.GetTopicById(topicId)??new Models.Topic();

            //话题图片
            List<StorageFile> imgList = _topicService.GetTopicFiles(topicId);
            imgList = imgList ?? new List<StorageFile>();
            ViewBag.TopicImages = imgList;
            return View(model);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.SetHotKey })]
        public ActionResult SetHot(Guid topicId)
        {
            var result = new DataJsonResult();
            
            if(!_topicService.SetHot(topicId))
                result.ErrorMessage = "设置失败！";

            return Json(result);
        }


    }
}