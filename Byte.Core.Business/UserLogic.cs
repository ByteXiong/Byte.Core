using Byte.Core.Repository;
using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using Byte.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Byte.Core.Common.Extensions;
using Mapster;
using System.Linq.Expressions;
using Byte.Core.Common.Filters;
using Byte.Core.Tools;
using LogicExtensions;
using NPOI.OpenXmlFormats.Dml;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Formatters;
using SqlSugar;

namespace Byte.Core.Business
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserLogic : BaseBusinessLogic<int, User, UserRepository>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserLogic(UserRepository repository,  IUnitOfWork unitOfWork) : base(repository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }






        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<UserDTO>> GetPageAsync([FromQuery] UserParam param)
        {
            Expression<Func<User, bool>> where = x => x.UserName!= AppConfig.Root;
            if (!string.IsNullOrWhiteSpace(param.KeyWord))
            {
                param.KeyWord = param.KeyWord.Trim();
                where = where.And(x => x.NickName.Contains(param.KeyWord));
            }

            if (param.DeptId != default)
            {
                where = where.And(x => x.Depts.Any(y=>y.Id== param.DeptId) );
            }
            var page = await GetIQueryable(where).Select<UserDTO>().SearchWhere(param).ToPagedResultsAsync(param);

            return page;
        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetInfoAsync(int id)
        {
            var entity = await GetIQueryable(x => x.Id == id)
                .Includes(x => x.Roles)
                .Select<UserInfo>(
                 x=> new UserInfo {
                  RoleIds= x.Roles.Select(y=>y.Id).ToList()
                 },true
                ).FirstAsync();
            return entity;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(UpdateUserParam param)
        {
            try
            {

                _unitOfWork.BeginTran();

                User model = param.Adapt<User>();

                model.Roles = param.RoleIds.Select(x => new Role { Id = x }).ToList();
                model.Password= "123456".ToMD5String();

                await _unitOfWork.GetDbClient().InsertNav(model)
                        .Include(z1 => z1.Roles, new InsertNavOptions
                        {
                            ManyToManyNoDeleteMap = true,
                             //设置中间表其他字段 （5.1.4.86)
                            ManyToManySaveMappingTemplate = new User_Dept_Role()
                           {
                                DeptId = CurrentUser.DeptId
                           }
                        })
                   .ExecuteCommandAsync();

                _unitOfWork.CommitTran();
                return model.Id;
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTran();

                throw;
            }


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(UpdateUserParam param)
        {

            try
            {

                _unitOfWork.BeginTran();

           var model =  new User
                {
                    Id = param.Id,

                        UserName = param.UserName, //账号
                     NickName = param.NickName, //名称
                    Avatar = param.Avatar, //头像
                    Password = param.Password, //密码
                    Status = param.Status, //状态
                    
                };
                model.Roles = param.RoleIds.Select(x => new Role { Id = x }).ToList();

               await _unitOfWork.GetDbClient().Deleteable<User_Dept_Role>(x => x.DeptId == CurrentUser.DeptId && x.UserId != model.Id && !param.RoleIds.Contains(x.RoleId)).ExecuteCommandAsync();
                await   _unitOfWork.GetDbClient().UpdateNav(model)
                 .Include(z1 => z1.Roles, new UpdateNavOptions
            {
                ManyToManyIsUpdateA = true,
                     //设置中间表其他字段 （5.1.4.86)
                     ManyToManySaveMappingTemplate = new User_Dept_Role()
                     {
                         DeptId = CurrentUser.DeptId
                     }
                 })
            .ExecuteCommandAsync();
                
                _unitOfWork.CommitTran();
                return param.Id;
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTran();

                throw;
            }
        }
        /// <summary>
        ///  设置状态
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> SetStatusAsync(int id, bool status) => await UpdateAsync(x => id == x.Id, x => new User { Status = status });




        public async Task<int> SetPassword(SetPasswordParam param)
        {
            var oldPwd = param.OldPassword.Md5();
            var newPwd = param.NewPassword.Md5();
            var any = await GetIQueryable(x => x.Id != param.Id && x.Password == oldPwd).AnyAsync();
            if (!any)
                throw new BusException("旧密码不正确");
            return await UpdateAsync(x => param.Id == x.Id, x => new User { Password = newPwd });

        }
    }
}