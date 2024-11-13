using Byte.Core.Common.Extensions;
using Byte.Core.Common.Filters;
using Byte.Core.Common.Helpers;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.Repository;
using Byte.Core.Tools.Attributes;
using System.Linq.Expressions;
using Byte.Core.Tools;
using Dm.filter;
namespace Byte.Core.Business
{
    public class LoginLogic(UserRepository userRepository, MenuRepository menuRepository, RoleRepository roleRepository)
    {
        /// <summary>
        /// 
        /// </summary>
         readonly UserRepository _userRepository= userRepository;
          readonly MenuRepository _menuRepository = menuRepository;
        readonly RoleRepository _roleRepository = roleRepository;
        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<LoginToken> LoginAsync(LoginParam param)
        {
            param.Password = param.Password.ToMD5String();
            Expression<Func<User, bool>> where = x => x.UserName == param.UserName && x.Password == param.Password;
#if DEBUG
            where = x => x.UserName == param.UserName;
#endif

            var user = await _userRepository.GetIQueryable(where)
                .Select(x => new  {

                Id = x.Id,
                UserName = x.UserName,
                //RoleCode = x.User_Dept_Roles.Select(y => y.Role.Code).ToList(),
                //Type = user.Role.Type,
                NickName = x.NickName,
                Status=x.Status
            } ).FirstAsync();
            if (user == null) throw new BusException("账号或密码错误");
            if (!user.Status) throw new BusException("账号已禁用");

            var jwt = new JWTPayload
            {
                Id = user.Id,
                UserName = user.UserName,
                RoleCodes = _roleRepository.GetIQueryable(x=>x.User_Dept_Roles.Any(y=>y.UserId == user.Id)).Select(x => x.Code).ToList(),
                //Type = user.Role.Type,
                NickName = user.NickName,
            };
            return await LoginTokenAsync(jwt);
        }



        /// <summary>
        /// 微信登录
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusException"></exception>
        public async Task<LoginToken> WeChatAsync()
        {
            Expression<Func<User, bool>> where = x => x.UserName == "admin";

            var entity = await _userRepository.GetIQueryable(where)
                            .Select(x => new JWTPayload
                            {
                                Expire = DateTime.Now.AddDays(30),
                            }, true).FirstAsync();

            if (entity == null) throw new BusException("没有查到用户信息");
            var str = ObjectExtension.ToJson(entity);
            string token = JWTHelper.SetToken(str, JWTHelper.JWTSecret);
            return await LoginTokenAsync(entity);
        }




        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<LoginInfoDTO> InfoAsync()
        {
            Expression<Func<User, bool>> where = x => x.Id == CurrentUser.Id;
            var entity = await _userRepository.GetIQueryable(where)
                   .Select<LoginInfoDTO>()
                   .FirstAsync();
          
            if (entity == null) throw new BusException("没有查到用户信息");

            entity.Buttons = await _menuRepository.GetPermAsync(CurrentUser.RoleCodes);
            return entity;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public async Task LoginOutAsync()
        {

        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="jwtPayload"></param>
        /// <returns></returns>
        private async Task<LoginToken> LoginTokenAsync(JWTPayload jwtPayload)
        {
            //jwtPayload.RoleCodes = "ROOT";
            jwtPayload.Expire = DateTime.Now.AddDays(30);
            //jwtPayload.Expire = DateTime.Now.AddSeconds(60);
            //entity.Roles = await _user_RoleLogic.GetIQueryable(x => x.UserId == entity.Id).Select(x => x.RoleId).ToArrayAsync();
            var str = jwtPayload.ToJson();
            string token = JWTHelper.SetToken(str, JWTHelper.JWTSecret);
            var loginToken = new LoginToken()
            {

                AccessToken = token,
                TokenType = "Bearer",
                Expires = (jwtPayload.Expire - DateTime.Now).Seconds,
            };
            return loginToken;
        }
    }
}
