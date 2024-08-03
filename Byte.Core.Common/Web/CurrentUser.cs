using Byte.Core.Common.Attributes;
using Byte.Core.Common.IoC;
using Byte.Core.Common.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Byte.Core.Common.Web
{
    /// <summary>
    /// 这个类是用来存储当前登录用户的 模版例子
    /// </summary>
    public static class CurrentUser
    {
        #region Initialize
        private static IHttpContextAccessor _context => ServiceLocator.Resolve<IHttpContextAccessor>();
        private static ISession _session => _context.HttpContext?.Session;



        public static void Configure(JWTPayloadBase param)
        {
            throw new NotImplementedException("这是模版例子 拷贝后重写");
            //Id = param.Id;
            //Account = param.Account;
            //Type= param.Type;
            //Name = param.Name;
            //DeptId = param.DeptId;
            ////Role = param.Role;
            //RoleCode = param.RoleCode;
        }



        #endregion

        #region Attribute

        //public static Guid Id

        //{
        //    get => _session == null ? default : Guid.Parse(_session.GetString("CurrentUser_Id"));
        //    set => _session.SetString("CurrentUser_Id", value.ToString());
        //}

        //public static string Account
        //{
        //    get => _session == null ? "" : _session.GetString("CurrentUser_Account");
        //    set => _session.SetString("CurrentUser_Account", !string.IsNullOrEmpty(value) ? value : "");
        //}

        //public static string Phone
        //{
        //    get => _session == null ? "" : _session.GetString("CurrentUser_Phone");
        //    set => _session.SetString("CurrentUser_Phone", !string.IsNullOrEmpty(value) ? value : "");
        //}
        //public static string Name
        //{
        //    get => _session == null ? "" : _session.GetString("CurrentUser_Name");
        //    set => _session.SetString("CurrentUser_Name", !string.IsNullOrEmpty(value) ? value : "");
        //}
        //public static Guid DeptId

        //{
        //    get => _session == null ? default : Guid.Parse(_session.GetString("CurrentUser_DeptId"));
        //    set => _session.SetString("CurrentUser_DeptId", value.ToString());
        //}

        //public static UserTypeEnum Type
        //{
        //    get => _session == null ? default : (UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), _session.GetString("CurrentUser_Type"));
        //    set => _session.SetString("CurrentUser_Type", value.ToString());
        //}
        //public static string RoleCode
        //{
        //    get => _session == null ? "" : _session.GetString("CurrentUser_RoleCode");
        //    set => _session.SetString("CurrentUser_RoleCode", !string.IsNullOrEmpty(value) ? value : "");
        //}

        #endregion
    }
}
