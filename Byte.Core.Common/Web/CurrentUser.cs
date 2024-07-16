using Byte.Core.Common.IoC;
using Byte.Core.Common.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Byte.Core.Common.Web
{
    public static class CurrentUser
    {
        #region Initialize
        private static IHttpContextAccessor _context => ServiceLocator.Resolve<IHttpContextAccessor>();
        private static ISession _session => _context.HttpContext?.Session;



        public static void Configure(JWTPayload param)
        {
            Id = param.Id;
            Account = param.Account;
            Type= param.Type;
            Name = param.Name;
            DeptId = param.DeptId;
            Role = param.Role;
        }
  


        #endregion

        #region Attribute

        public static Guid Id

        {
            get => _session == null ? default : Guid.Parse(_session.GetString("CurrentUser_Id"));
            set => _session.SetString("CurrentUser_Id", value.ToString());
        }

        public static string Account
        {
            get => _session == null ? "" : _session.GetString("CurrentUser_Account");
            set => _session.SetString("CurrentUser_Account", !string.IsNullOrEmpty(value) ? value : "");
        }

        public static string Phone
        {
            get => _session == null ? "" : _session.GetString("CurrentUser_Phone");
            set => _session.SetString("CurrentUser_Phone", !string.IsNullOrEmpty(value) ? value : "");
        }
        public static string Name
        {
            get => _session == null ? "" : _session.GetString("CurrentUser_Name");
            set => _session.SetString("CurrentUser_Name", !string.IsNullOrEmpty(value) ? value : "");
        }
        public static Guid DeptId

        {
            get => _session == null ? default : Guid.Parse(_session.GetString("CurrentUser_DeptId"));
            set => _session.SetString("CurrentUser_DeptId", value.ToString());
        }

        public static UserType Type
        {
            get => _session == null ? default : (UserType)Enum.Parse(typeof(UserType), _session.GetString("CurrentUser_Type"));
            set => _session.SetString("CurrentUser_Type", value.ToString());
        }
        public static string Role
        {
            get => _session == null ? "" : _session.GetString("CurrentUser_Role");
            set => _session.SetString("CurrentUser_Role", !string.IsNullOrEmpty(value) ? value : "");
        }

        #endregion
    }
}
