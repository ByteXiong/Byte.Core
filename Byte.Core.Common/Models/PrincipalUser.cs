using Newtonsoft.Json;
using System.Security.Claims;

namespace Byte.Core.Common.Models
{
    [JsonObject]
    public class PrincipalUser
    {

        [JsonProperty]
        public string UserName
        {
            get;
            set;
        }


        [JsonProperty]
        public string NickName
        {
            get;
            set;
        }


        [JsonProperty]
        public string Email
        {
            get;
            set;
        }


        [JsonProperty]
        public string Mobile
        {
            get;
            set;
        }


        [JsonProperty]
        public string Avatar
        {
            get;
            set;
        }


        [JsonProperty]
        public string Code
        {
            get;
            set;
        }

        [JsonProperty]
        public int? No
        {
            get;
            set;
        }

        [JsonProperty]
        public Guid Id
        {
            get;
            set;
        }


        [JsonProperty]
        public int? Role
        {
            get;
            set;
        }


        [JsonProperty]
        public string RoleName
        {
            get;
            set;
        }

        [JsonIgnore]
        public string ClientIP
        {
            get;
            set;
        }
        public ClaimsPrincipal CurrentClaimsPrincipal
        {
            get;
            protected set;
        }

        public PrincipalUser()
        {
            Id = Guid.Empty;
            UserName = "Admin";
            NickName = "匿名用户";
            Email = "123456@qq.com";
            Mobile = "13000000000";
            Code = "0000001";
            Role = null;
        }

        public virtual void SetClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            CurrentClaimsPrincipal = claimsPrincipal;
            PrincipalUser principalUser = JsonConvert.DeserializeObject<PrincipalUser>(CurrentClaimsPrincipal?.FindFirst("baseInfo")?.Value);
            Guid.TryParse(CurrentClaimsPrincipal?.FindFirst("sub")?.Value, out Guid result);
            Id = result;
            UserName = principalUser.UserName;
            NickName = principalUser.NickName;
            Email = principalUser.Email;
            Mobile = principalUser.Mobile;
            Avatar = principalUser.Avatar;
            Code = principalUser.Code;
            No = principalUser.No;
            Role = principalUser.Role;
            RoleName = principalUser.RoleName;
        }

        //
        // 摘要:
        //     获取匿名用户
        public static PrincipalUser GetAnonymousUser<T>() where T : PrincipalUser, new()
        {
            return new T();
        }
    }
}
