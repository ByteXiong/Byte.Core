using Byte.Core.Common.IoC;
using Byte.Core.Tools;
using Byte.Core.Tools.Attributes;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Byte.Core.Tools
{
    public static class CurrentUser
    {
        #region Initialize
        private static IHttpContextAccessor _context => ServiceLocator.Resolve<IHttpContextAccessor>();
        private static ISession _session => _context.HttpContext?.Session;



        public static void Configure(JWTPayload param)
        {
            Id = param.Id;
            UserName = param.UserName;
            RoleType = param.RoleType;
            NickName = param.NickName;
            DeptId = param.DeptId;
            //Role = param.Role;
            RoleCodes = param.RoleCodes;
        }



        #endregion

        #region Attribute

        public static long Id

        {
            get => _session == null ? default : long.Parse(_session.GetString("CurrentUser_Id"));
            set => _session.SetString("CurrentUser_Id", value.ToString());
        }

        public static string UserName
        {
            get => _session == null ? "" : _session.GetString("CurrentUser_UserName");
            set => _session.SetString("CurrentUser_UserName", !string.IsNullOrEmpty(value) ? value : "");
        }

        public static string Phone
        {
            get => _session == null ? "" : _session.GetString("CurrentUser_Phone");
            set => _session.SetString("CurrentUser_Phone", !string.IsNullOrEmpty(value) ? value : "");
        }
        public static string NickName
        {
            get => _session == null ? "" : _session.GetString("CurrentUser_NickName");
            set => _session.SetString("CurrentUser_NickName", !string.IsNullOrEmpty(value) ? value : "");
        }
        //public static Guid DeptId

        //{
        //    get => _session == null ? default : Guid.Parse(_session.GetString("CurrentUser_DeptId"));
        //    set => _session.SetString("CurrentUser_DeptId", value.ToString());
        //}

        public static long DeptId

        {
            get => _session == null ? default : long.Parse(_session.GetString("CurrentUser_DeptId"));
            set => _session.SetString("CurrentUser_DeptId", value.ToString());
        }

        public static RoleTypeEnum? RoleType
        {
            get => _session == null ? default : (RoleTypeEnum)Enum.Parse(typeof(RoleTypeEnum), _session.GetString("CurrentUser_RoleType"));
            set => _session.SetString("CurrentUser_RoleType", value.ToString());
        }
        public static List<string> RoleCodes
        {
            get => _session == null ? new List<string>  { } : _session.GetString("CurrentUser_RoleCode")?.Split(',')?.ToList();
            set => _session.SetString("CurrentUser_RoleCode", string.Join(",", value??new List<string> { }));
        }

        #endregion
    }
}
