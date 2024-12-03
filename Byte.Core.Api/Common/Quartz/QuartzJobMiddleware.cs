using Byte.Core.Business.Quartz;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.IoC;
using Byte.Core.Entity;
using Byte.Core.SqlSugar.IDbContext;
using Byte.Core.Tools.Extensions;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.Formula.Functions;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Quartz.Logging.OperationName;
using static Thrift.Protocols.Utilities.TJSONProtocolConstants;

namespace Byte.Core.Api.Common.Quartz
{
    public static class QuartzJobMiddleware
    {

        public static async Task QuartzJobInit(this IApplicationBuilder app)
        {
            var _scheduler = app.ApplicationServices.GetService<IScheduler>();
            _scheduler.ListenerManager.AddSchedulerListener(new MySchedulerListener());
            _scheduler.ListenerManager.AddJobListener(new MyJobListener());
            _scheduler.ListenerManager.AddTriggerListener(new MyTriggerListener());
            await _scheduler.Start();
            var _jobDetail = app.ApplicationServices.GetService<JobDetailLogic>();
            await _jobDetail.ScheduleJobs();
        }







    }

}

