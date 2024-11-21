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

namespace Byte.Core.Business
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserLogic : BaseBusinessLogic<int, User, UserRepository>
    {
        private readonly User_Dept_RoleRepository _user_Dept_RoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserLogic(UserRepository repository, User_Dept_RoleRepository user_Dept_RoleRepository, IUnitOfWork unitOfWork) : base(repository)
        {
            _user_Dept_RoleRepository = user_Dept_RoleRepository ?? throw new ArgumentNullException(nameof(User_Dept_RoleRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }






        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<UserDTO>> GetPageAsync([FromQuery] UserParam param)
        {
            Expression<Func<User, bool>> where = x => x.UserName!= AppConfig.Admin;
            if (!string.IsNullOrWhiteSpace(param.KeyWord))
            {
                param.KeyWord = param.KeyWord.Trim();
                where = where.And(x => x.NickName.Contains(param.KeyWord));
            }

            if (param.DeptId != default)
            {
                where = where.And(x => x.User_Dept_Roles.Any(y=>y.DeptId== param.DeptId) );
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
            var entity = await GetIQueryable(x => x.Id == id).Select<UserInfo>().FirstAsync();
            entity.RoleIds= await  _user_Dept_RoleRepository.GetIQueryable(x => x.UserId == id).Select(x=>x.RoleId).ToArrayAsync();
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

                await AddAsync(model);

                List<User_Dept_Role> User_Dept_Roles = new List<User_Dept_Role>();
                param.RoleIds.ForEach(x =>
                {
                    User_Dept_Roles.Add(new User_Dept_Role
                    {
                        UserId = model.Id,
                        DeptId = 1,
                        RoleId = x
                    });
                });
                await _user_Dept_RoleRepository.AddRangeAsync(User_Dept_Roles);
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

                await UpdateAsync(x => x.Id == param.Id, x => new User
                {
                    NickName = param.NickName, //名称
                    Avatar = param.Avatar, //头像
                    Password = param.Password, //密码
                    CreateTime = param.CreateTime, //创建时间
                    CreateBy = param.CreateBy, //创建人
                    Status = param.Status, //状态
                    UserName = param.UserName, //账号

                });


                List<User_Dept_Role> User_Dept_Roles = new List<User_Dept_Role>();
                await _user_Dept_RoleRepository.DeleteAsync(x => x.UserId == param.Id);
                param.RoleIds?.ForEach(x =>
                {
                    User_Dept_Roles.Add(new User_Dept_Role
                    {
                        UserId = param.Id,
                        DeptId = 1,
                        RoleId = x
                    });
                });
                await _user_Dept_RoleRepository.AddRangeAsync(User_Dept_Roles);
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