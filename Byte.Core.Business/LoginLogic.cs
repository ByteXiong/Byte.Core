using Byte.Core.Common.Extensions;
using Byte.Core.Common.Filters;
using Byte.Core.Common.Helpers;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.Repository;
using Byte.Core.Tools.Attributes;
using System.Linq.Expressions;
using Byte.Core.Tools;
namespace Byte.Core.Business
{
    public class LoginLogic(UserRepository userRepository)
    {
        /// <summary>
        /// 
        /// </summary>
         readonly UserRepository _userRepository= userRepository;

        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<LoginToken> LoginAsync(LoginParam param)
        {
            param.Password = param.Password.ToMD5String();
            Expression<Func<User, bool>> where = x => x.Account == param.Account && x.Password == param.Password;
#if DEBUG
            where = x => x.Account == param.Account;
#endif

            var user = await _userRepository.GetIQueryable(where).FirstAsync();
            if (user == null) throw new BusException("账号或密码错误");
            if (!user.State) throw new BusException("账号已禁用");

            var jwt = new JWTPayload
            {
                Id = user.Id,
                Account = user.Account,
                //RoleCode = user.Role.Code,
                //Type = user.Role.Type,
                Name = user.Name,
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
            Expression<Func<User, bool>> where = x => x.Account == "admin";

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
                   .Select<LoginInfoDTO>().FirstAsync();
            if (entity == null) throw new BusException("没有查到用户信息");
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
            jwtPayload.RoleCode = "ROOT";
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
