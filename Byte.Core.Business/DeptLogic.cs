using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using Byte.Core.Repository;
using Byte.Core.Models;
using Mapster;
using Byte.Core.Tools;
namespace Byte.Core.Business
{
    /// <summary>
    /// 部门
    /// </summary>
    public class DeptLogic : BaseBusinessLogic<long, Dept, DeptRepository>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public DeptLogic(DeptRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// 树图
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeptTreeDTO>> GetTreeAsync()
        {
            var tree = await GetIQueryable().OrderByDescending(x => x.Sort).Select<DeptTreeDTO>().ToTreeAsync(it => it.Children, it => it.ParentId, null, it => it.Id);
            return tree;
        }

        /// <summary>
        /// 下拉框
        /// </summary>
        /// <param name="types"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<List<DeptSelectDTO>> GetTreeSelectAsync(DeptTypeEnum[] types = null, long? parentId = null)
        {
            var tree = await GetIQueryable()
                .WhereIF(types != null, x => types.Contains(x.Type))
                .OrderByDescending(x => x.Sort).Select<DeptSelectDTO>().ToTreeAsync(it => it.Children, it => it.ParentId, parentId, it => it.Id);
            return tree;
        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeptInfo> GetInfoAsync(long id)
        {
            var entity = await GetIQueryable(x => x.Id == id).Select<DeptInfo>().FirstAsync();
            return entity;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> AddAsync(UpdateDeptParam param)
        {
            Dept model = param.Adapt<Dept>();
            await AddAsync(model);
            return model.Id;
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> UpdateAsync(UpdateDeptParam param)
        {
            var model = await GetIQueryable(x => x.Id == param.Id).FirstAsync();
            model.Name = param.Name;
            model.Type = param.Type;
            model.EasyName = param.EasyName;
            model.Image = param.Image;
            model.ParentId = param.ParentId;
            model.Status = param.Status;
            model.Sort = param.Sort;
            await UpdateAsync(model);
            return model.Id;
        }
        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] ids) => await DeleteAsync(x => ids.Contains(x.Id));

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<int> SetStatusAsync(long id, bool status) => await UpdateAsync(x => id == x.Id, x => new Dept { Status = status });
    }
}