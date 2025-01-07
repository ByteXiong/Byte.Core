using Byte.Core.Common.Helpers;
using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Repository
{
    public class SettingRepository : BaseRepository<long, Setting>
    {

        private readonly ILogger<SettingRepository> _logger;

        public SettingRepository(IUnitOfWork unitOfWork, ILogger<SettingRepository> logger) : base(unitOfWork)
        {
          _logger=logger;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public async Task<T> GetSettingValue<T>(string settingName)
        {
            var settingList = await    GetIQueryable().WithCache(86400).ToListAsync();

            var setting = settingList.FirstOrDefault(x => x.Name == settingName.Trim());
            if (setting == null) return default;

            try
            {
                return (T)ConvertValue(typeof(T), setting.Value);
            }
            catch (Exception e)
            {
                _logger.LogError(ExceptionHelper.GetExceptionAllMsg(e));
                return default;
            }
        }

        private static object ConvertValue(Type type, string value)
        {
            if (type == typeof(object))
            {
                return value;
            }

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return string.IsNullOrEmpty(value) ? value : ConvertValue(Nullable.GetUnderlyingType(type), value);
            }

            var converter = TypeDescriptor.GetConverter(type);
            return converter.CanConvertFrom(typeof(string)) ? converter.ConvertFromInvariantString(value) : null;
        }
    }
}