using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BntWeb.Caching;
using BntWeb.Logging;
using BntWeb.Security;

namespace BntWeb.Dashboard.Controllers
{
    [AdminAuthorize]
    public class StatisticsController : Controller
    {
        private readonly ICacheManager _cacheManager;
        public StatisticsController(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }


    }
}