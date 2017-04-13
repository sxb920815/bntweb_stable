/* 
    ======================================================================== 
        File name：		ActivityOperateTask
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/23 15:46:30
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using BntWeb.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Activity.Models;
using BntWeb.Logging;

namespace BntWeb.Activity.Services
{
    public class ActivityOperateTask : IBackgroundTask
    {
        private readonly IActivityService _activityService;

        public ActivityOperateTask(IActivityService activityService)
        {
            _activityService = activityService;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void Sweep()
        {
            //StartTime到时更新为进行中
            var count = _activityService.SetActivitysStatus(ActivityStatus.Doing);
            if (count > 0)
                Logger.Warning("{0}个活动自动转为进行中", count);

            count = _activityService.SetActivitysStatus(ActivityStatus.Finish);
            if (count > 0)
                Logger.Warning("{0}个活动自动转为已结束", count);
        }
    }
}