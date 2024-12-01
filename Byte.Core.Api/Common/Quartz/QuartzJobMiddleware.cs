using Byte.Core.Common.Extensions;
using Byte.Core.Common.IoC;
using Byte.Core.Entity;
using Byte.Core.Repository;
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
            //IScheduler _scheduler = ServiceLocator.Resolve<IScheduler>();
            _scheduler.ListenerManager.AddSchedulerListener(new SchedulerListener());
            _scheduler.ListenerManager.AddJobListener(new JobListener());
            _scheduler.ListenerManager.AddTriggerListener(new TriggerListener());
           await _scheduler.Start();

                var dataContext = ServiceLocator.Resolve<SugarDbContext>();
                //获取数据库的QZ配置
                var list = dataContext.Db.Queryable<JobDetail>().Includes(x => x.Triggers).ToList();
                //先判断_scheduler 是否存在
                var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
                Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>> triggersAndJobs = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();
                // Get the job detail for each job
                foreach (JobDetail detail in list)
                {
                  
                        //await _scheduler.DeleteJob(jobDetail.Key);
                        JobDataMap dataMap = new JobDataMap();
                        var props = detail.Props?.ToObject<Dictionary<string, object>>();
                props?.ForEach(x => dataMap.Put(x.Key, x.Value));

                Assembly assIBll = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "/Byte.Core.Api.dll");
                //加载dll后,需要使用dll中某类.
                Type type = assIBll.GetType($"Byte.Core.Api.Common.Quartz.{detail.AssemblyName}");//获取类名，必须 命名空间+类名 
               
                IJobDetail jobDetail = JobBuilder.Create(type)
                        .WithIdentity(detail.Name)
                         .SetJobData(dataMap)
                         .StoreDurably()
                        .Build();
                    //Console.WriteLine($"Job Name: {jobKey.Name}, Group Name: {jobKey.Group}");
                    await _scheduler.AddJob(jobDetail, true);
                 
                    List<ITrigger> triggers = new List<ITrigger>();
                    foreach (JobTrigger trigger in detail.Triggers)
                    {
                   
                         JobDataMap triggerMap = new JobDataMap();
                        var triggerProps = trigger.Props?.ToObject<Dictionary<string, object>>();
                        triggerProps?.ForEach(x => triggerMap.Put(x.Key, x.Value));

                        ITrigger jobTrigger = TriggerBuilder.Create()
                        .WithIdentity(trigger.Name)
                        .UsingJobData(triggerMap)
                        .ForJob(jobDetail.Key)
                        .StartNow()
                        .WithCronSchedule(trigger.TriggerType)
                        .Build();
                        triggers.Add(jobTrigger);
                            //await _scheduler.AddJob(jobDetail, false);
                            // 调度任务


                        //Console.WriteLine($"  Trigger Name: {trigger.Key.Name}, Group Name: {trigger.JobKey.Name}");

                    }
                    triggersAndJobs.Add(jobDetail, triggers);
               
                }
                await _scheduler.ScheduleJobs(triggersAndJobs, true);
            }
        
       





        }

    }

