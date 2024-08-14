using Byte.Core.Repository;
using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using Byte.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Byte.Core.Common.Extensions;
using Mapster;
using System.Linq.Expressions;
using Byte.Core.Tools;

namespace Byte.Core.Business
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserLogic : BaseBusinessLogic<Guid, User, UserRepository>
    {
        private readonly User_Dept_RoleLogic _User_Dept_RoleLogic;
        private readonly IUnitOfWork _unitOfWork;
        public UserLogic(UserRepository repository, User_Dept_RoleLogic user_Dept_RoleLogic, IUnitOfWork unitOfWork) : base(repository)
        {
            _User_Dept_RoleLogic = user_Dept_RoleLogic ?? throw new ArgumentNullException(nameof(User_Dept_RoleLogic));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }






        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<UserDTO>> GetPageAsync([FromQuery] UserParam param)
        {
            Expression<Func<User, bool>> where = x => x.Account!= ParamConfig.Admin;
            if (!string.IsNullOrWhiteSpace(param.KeyWord))
            {
                param.KeyWord = param.KeyWord.Trim();
                where = where.And(x => x.Name.Contains(param.KeyWord));
            }

            if (param.DeptId != default)
            {
                where = where.And(x => x.Depts.Any(y=>y.Id== param.DeptId) );
            }

            var page = await GetIQueryable(where).Select<UserDTO>().ToPagedResultsAsync(param);

            return page;
        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetInfoAsync(Guid id)
        {
            var entity = await GetIQueryable(x => x.Id == id).Select<UserInfo>().FirstAsync();
            return entity;
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Guid> AddAsync(UpdateUserParam param)
        {
            try
            {

                _unitOfWork.BeginTran();

                User model = param.Adapt<User>();

                await AddAsync(model);

                List<User_Dept_Role> User_Dept_Roles = new List<User_Dept_Role>();
                param.RoleIds.ForEach(x =>
                {
                    User_Dept_Roles.Add(new User_Dept_Role
                    {
                        UserId = model.Id,
                        RoleId = x
                    });
                });
                await _User_Dept_RoleLogic.AddRangeAsync(User_Dept_Roles);
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
        public async Task<Guid> UpdateAsync(UpdateUserParam param)
        {

            try
            {

                _unitOfWork.BeginTran();

                await UpdateAsync(x => x.Id == param.Id, x => new User
                {
                    Name = param.Name, //名称
                    Avatar = param.Avatar, //头像
                    Password = param.Password, //密码
                    CreateTime = param.CreateTime, //创建时间
                    CreateBy = param.CreateBy, //创建人
                    State = param.State, //状态

                    Account = param.Account, //账号

                });


                List<User_Dept_Role> User_Dept_Roles = new List<User_Dept_Role>();
                param.RoleIds?.ForEach(x =>
                {
                    User_Dept_Roles.Add(new User_Dept_Role
                    {
                        UserId = param.Id,
                        RoleId = x
                    });
                });
                await _User_Dept_RoleLogic.AddRangeAsync(User_Dept_Roles);
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
        public async Task<int> SetStateAsync(Guid id, bool state) => await UpdateAsync(x => id == x.Id, x => new User { State = state });
    }
}