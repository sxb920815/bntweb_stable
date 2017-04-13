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
using BntWeb.MemberBase;
using BntWeb.MemberBase.Models;
using BntWeb.MemberBase.Services;
using BntWeb.MemberCenter.ViewModels;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Security.Identity;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.Web.Extensions;

namespace BntWeb.MemberCenter.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IStorageFileService _storageFileService;

        public AdminController(IMemberService memberService, IStorageFileService storageFileService)
        {
            _memberService = memberService;
            _storageFileService = storageFileService;
        }


        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMemberKey })]
        public ActionResult Edit(string id)
        {
            Argument.ThrowIfNullOrEmpty(id, "会员Id不能为空");
            var member = _memberService.FindMemberById(id);

            ViewBag.AvatarFile =
                _storageFileService.GetFiles(member.Id.ToGuid(), MemberBaseModule.Key, "Avatar").FirstOrDefault();
            return View(member);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMemberKey })]
        public ActionResult EditOnPost(EditMemberViewModel editMember)
        {
            var result = new DataJsonResult();
            Member oldMember = null;
            if (!string.IsNullOrWhiteSpace(editMember.MemberId))
                oldMember = _memberService.FindMemberById(editMember.MemberId);

            if (oldMember == null)
            {
                //新建用户
                User user = _memberService.FindUserByName(editMember.UserName);
                if (user == null)
                {
                    var member = new Member
                    {
                        UserName = editMember.UserName,
                        Email = editMember.Email,
                        PhoneNumber = editMember.PhoneNumber,
                        LockoutEnabled = false,
                        NickName = editMember.NickName,
                        Birthday = editMember.Birthday,
                        Sex = editMember.Sex,
                        Province = editMember.Member_Province,
                        City = editMember.Member_City,
                        District = editMember.Member_District,
                        Street = editMember.Member_Street,
                        Address = editMember.Address
                    };

                    var identityResult = _memberService.CreateMember(member, editMember.Password);

                    if (!identityResult.Succeeded)
                    {
                        result.ErrorMessage = identityResult.Errors.FirstOrDefault();
                    }
                    else
                    {
                        _storageFileService.AssociateFile(member.Id.ToGuid(), MemberBaseModule.Key, MemberBaseModule.DisplayName, editMember.Avatar.ToGuid(), "Avatar");
                    }
                }
                else
                {
                    result.ErrorMessage = "此用户名的账号已经存在！";
                }
            }
            else
            {
                //编辑用户
                oldMember.Email = editMember.Email;
                oldMember.PhoneNumber = editMember.PhoneNumber;

                oldMember.NickName = editMember.NickName;
                oldMember.Birthday = editMember.Birthday;
                oldMember.Sex = editMember.Sex;
                oldMember.Province = editMember.Member_Province;
                oldMember.City = editMember.Member_City;
                oldMember.District = editMember.Member_District;
                oldMember.Street = editMember.Member_Street;
                oldMember.Address = editMember.Address;

                var identityResult = _memberService.UpdateMember(oldMember, editMember.Password, editMember.Password2);
                if (!identityResult.Succeeded)
                {
                    result.ErrorMessage = identityResult.Errors.FirstOrDefault();
                }
                else
                {
                    _storageFileService.ReplaceFile(oldMember.Id.ToGuid(), MemberBaseModule.Key, MemberBaseModule.DisplayName, editMember.Avatar.ToGuid(), "Avatar");
                }
            }

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMemberKey })]
        public ActionResult List()
        {
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMemberKey })]
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
            var userName = Request.Get("extra_search[UserName]");
            var checkUserName = string.IsNullOrWhiteSpace(userName);
            
            var nickName = Request.Get("extra_search[NickName]");
            var checkNickName = string.IsNullOrWhiteSpace(nickName);

            var sex = Request.Get("extra_search[Sex]");
            var checkSex = string.IsNullOrWhiteSpace(sex);
            var sexInt = sex.To<int>();

             var createTimeBegin = Request.Get("extra_search[CreateTimeBegin]");
            var checkCreateTimeBegin = string.IsNullOrWhiteSpace(createTimeBegin);
            var createTimeBeginTime = createTimeBegin.To<DateTime>();

            var createTimeEnd = Request.Get("extra_search[CreateTimeEnd]");
            var checkCreateTimeEnd = string.IsNullOrWhiteSpace(createTimeEnd);
            var createTimeEndTime = createTimeEnd.To<DateTime>();

            Expression<Func<Member, bool>> expression =
                l => (checkUserName || l.UserName.Contains(userName)) &&
                     (checkNickName || l.NickName.Equals(nickName, StringComparison.OrdinalIgnoreCase)) &&
                     (checkSex || (int)l.Sex == sexInt) &&
                     (checkCreateTimeBegin || l.CreateTime >= createTimeBeginTime) &&
                     (checkCreateTimeEnd || l.CreateTime <= createTimeEndTime) &&
                     l.UserType == UserType.Member;

            Expression<Func<Member, object>> orderByExpression;
            //设置排序
            switch (sortColumn)
            {
                case "Birthday":
                    orderByExpression = u => new { u.Birthday };
                    break;
                case "Sex":
                    orderByExpression = u => new { u.Sex };
                    break;
                case "NickName":
                    orderByExpression = u => new { u.NickName };
                    break;
                case "CreateTime":
                    orderByExpression = u => new { u.CreateTime };
                    break;
                case "PhoneNumber":
                    orderByExpression = u => new { u.PhoneNumber };
                    break;
                case "Email":
                    orderByExpression = u => new { u.Email };
                    break;
                case "LockoutEnabled":
                    orderByExpression = u => new { u.LockoutEnabled };
                    break;
                default:
                    orderByExpression = u => new { u.UserName };
                    break;
            }

            //分页查询
            var members = _memberService.GetListPaged(pageIndex, pageSize, expression, orderByExpression, isDesc, out totalCount);

            result.data = members;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMemberKey })]
        public ActionResult Switch(SwitchMemberViewModel switchUser)
        {
            var result = new DataJsonResult();
            Member member = _memberService.FindMemberById(switchUser.MemberId);

            if (member != null)
            {
                if (member.UserName.Equals("bocadmin", StringComparison.OrdinalIgnoreCase))
                {
                    result.ErrorMessage = "内置账号不可以禁用！";
                }
                else
                {
                    var identityResult = _memberService.SetLockoutEnabled(member.Id, switchUser.Enabled);

                    if (!identityResult.Succeeded)
                    {
                        result.ErrorMessage = identityResult.Errors.FirstOrDefault();
                    }
                }
            }

            else
            {
                result.ErrorMessage = "此用户名的账号不存在！";
            }

            return Json(result);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.DeleteMemberKey })]
        public async Task<ActionResult> Delete(string memberId)
        {
            var result = new DataJsonResult();
            Member member = _memberService.FindMemberById(memberId);

            if (member != null)
            {
                if (member.UserName.Equals("bocadmin", StringComparison.OrdinalIgnoreCase))
                {
                    result.ErrorMessage = "内置账号不可以删除！";
                }
                else
                {
                    var identityResult = await _memberService.Delete(member);

                    if (!identityResult.Succeeded)
                    {
                        result.ErrorMessage = identityResult.Errors.FirstOrDefault();
                    }
                }
            }
            else
            {
                result.ErrorMessage = "此用户名的账号不存在！";
            }

            return Json(result);
        }
    }

}