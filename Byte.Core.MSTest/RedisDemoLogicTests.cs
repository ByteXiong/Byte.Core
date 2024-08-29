using Byte.Core.Common.Extensions;
using Byte.Core.Common.IoC;
using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Mapster;
using SqlSugar;

namespace Byte.Core.Business.Tests
{
    [TestClass()]
    public class RedisDemoLogicTests
    {

        public RedisDemoLogic _logic;

        public IUnitOfWork _unitOfWork;
        [TestInitialize]
        public void Setup()
        {
            Byte.Core.MSTest.Program.Setup();
            _logic = ServiceLocator.Resolve<RedisDemoLogic>();
            _unitOfWork = ServiceLocator.Resolve<IUnitOfWork>();
        }

        [TestMethod()]
        public async Task GetByIdsTest()
        {

            //_roleLogic.AddInterceptor("DoSomething", new MyInterceptor());

            // 调用DoSomething方法
            await RedisHelper.HSetAsync(ParamConfig.HRedisDemoKey, "DF6CA8BA-F204-4622-BD3F-000061E4E567", "张三");

            var aa = await RedisHelper.HMGetAsync(ParamConfig.HRedisDemoKey, "DF6CA8BA-F204-4622-BD3F-000061E4E567");

            var result = await _logic.GetByIdAsync("DF6CA8BA-F204-4622-BD3F-000061E4E567".ToGuid().Value);
            //var result = await _logic.GetByIdsAsync(new Guid[] { Guid.NewGuid(), Guid.NewGuid() });

            Assert.IsTrue(result != null);
        }

        [TestMethod()]
        public async Task GetAddRedisTest()
        {
            //  var l =  await _unitOfWork.GetDbClient() .Queryable<Role,Dept>((r,d) => new JoinQueryInfos(JoinType.Left, r.DeptId == d.Id  ))
            // .WithCache("AAAA").Select<Role>(x=>new Role { Dept = x.Dept },true)
            //.ToListAsync();


            var list =
                await _unitOfWork.GetDbClient()
                .Queryable<Role>()
                .Includes(x => x.Dept)
                .WithCache("aaa")
                .ToListAsync();

            var list2 =
           await _unitOfWork.GetDbClient()
          .Queryable<Role>()
          .Includes(x => x.Dept)
          .Select(x => new Role { Name = x.Name, Dept = new Dept { Name = x.Dept.Name } })
          .WithCache("bbb").ToListAsync();

            var list3 =
             await _unitOfWork.GetDbClient()
            .Queryable<Role>()
            .Includes(x => x.Dept)
            .Select(x => new Role { Name = x.Name, Dept = x.Dept })
            .WithCache("ccc").ToListAsync();
            Assert.IsTrue(true);
        }




    }
}