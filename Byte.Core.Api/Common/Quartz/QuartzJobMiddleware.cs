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
            var _jobDetail = app.ApplicationServices.GetService<JobDetailLogic>();
            await  _jobDetail.StratAsync();
        }
        
       





        }

    }

