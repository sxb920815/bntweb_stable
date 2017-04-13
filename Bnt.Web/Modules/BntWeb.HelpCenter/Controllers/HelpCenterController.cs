using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BntWeb.HelpCenter.Services;
using BntWeb.Data.Services;
using BntWeb.HelpCenter.Models;
using BntWeb.Validation;


namespace BntWeb.HelpCenter.Controllers
{
    public class HelpCenterController : Controller
    {
        private readonly IHelpCenterService _helpCenterService;

        public HelpCenterController(IHelpCenterService helpCenterService, ICurrencyService currencyService)
        {
            _helpCenterService = helpCenterService;
        }

        //GET:H5
        public ViewResult HelpInfo(Guid id)
        {
            Argument.ThrowIfNull(id.ToString(), "Id");

            Help help = _helpCenterService.GetOneHelpById(id);
            Argument.ThrowIfNull(help, "信息不存在");

            return View(help);
        }
    }
}