using Byte.Core.Common.IoC;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using NPOI.SS.Formula.Functions;
using Quartz.Util;

namespace Byte.Core.Business.Tests
{
    [TestClass()]
    public class RoleLogicTests
    {
        public RoleLogic _roleLogic;
        public IUnitOfWork _unitOfWork;
        [TestInitialize]
        public void Setup()
        {
            Byte.Core.MSTest.Program.Setup();
            _roleLogic = ServiceLocator.Resolve<RoleLogic>();
            _unitOfWork = ServiceLocator.Resolve<IUnitOfWork>();


        }

    

        [TestMethod()]
        public async Task DeleteRedisAsyncTest()
        {
            //_unitOfWork.GetDbClient().DataCache.RemoveDataCache("RoleSelect");
            _unitOfWork.GetDbClient().Deleteable<Role>().RemoveDataCache("MyCackey").Where(it => true).ExecuteCommand();
            Assert.IsTrue(true);
        }
    }
}