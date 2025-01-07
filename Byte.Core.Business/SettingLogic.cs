using Byte.Core.Entity;
using Byte.Core.Repository;
using Byte.Core.SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Business
{
    public class SettingLogic : BaseBusinessLogic<long, Setting, SettingRepository>
    {
        /// <summary />
        /// <param name="repository"></param>
        public SettingLogic(SettingRepository repository) : base(repository) { }


        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public async Task<T> GetSettingValue<T>(string settingName) => await Repository.GetSettingValue<T>(settingName);

    }
}
