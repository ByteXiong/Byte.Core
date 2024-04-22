using Byte.Core.Common.Helpers;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Common.Attributes
{
    public class HostService : IHostedService, IDisposable
    {
        //定义一个定时器
        private Timer _timer;

        /// <summary>
        /// 启动任务绑定
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {

            //Console.WriteLine("启动" + DateTime.Now.ToString());

            Log4NetHelper.WriteInfo(typeof(HostService), "服务启动");
            Console.WriteLine("启动服务" + DateTime.Now.ToString());
           
            //绑定定时任务
            //设置延迟时间
            //_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60 * Interval));
            return Task.CompletedTask;
        }

        /// <summary>
        /// 定时执行的操作，绑定到定时器上
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private void DoWork(object state)
        {
            //Common.WriteEmailLog("定时任务被触发", "开始一波邮件发送");
            //try
            //{
            //    //一波操作
            //}
            //catch (Exception ex)
            //{
            //    Common.WriteEmailLog("定时发送邮件时报错", ex.Message);
            //}
        }

        /// <summary>
        /// 任务关闭时执行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        { 
             
            Log4NetHelper.WriteInfo(typeof(HostService), "服务被关闭");
            Console.WriteLine("服务被关闭" + DateTime.Now.ToString());
            //_timer?.Change(Timeout.Inf;inite, 0)
            return Task.CompletedTask;
        }

        /// <summary>
        /// 释放托管资源，释放时触发
        /// </summary>
        public  void Dispose()
        {
            //Common.WriteEmailLog("定时任务被释放闭", "...Dispose...");

            Log4NetHelper.WriteInfo(typeof(HostService), "服务被释放闭");
            Console.WriteLine("服务被释放闭" + DateTime.Now.ToString());
            IHostApplicationLifetime applicationLifetime;
            //_timer?.Dispose();
            //iis会回收这个定时任务，这边在回收的时候触发一个请求，来再次唤醒该服务
            Thread.Sleep(5000);
            HttpHelper.GetData("http://localhost:4050/API/Quartz/");
        }
    }
}
