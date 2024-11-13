using Microsoft.VisualStudio.TestTools.UnitTesting;
using Byte.Core.Business;
using Byte.Core.SqlSugar;
using Byte.Core.Common.IoC;
using System.Data;

namespace Byte.Core.Business.Tests
{
    [TestClass()]
    public class DemoTest
    {
        public DataTableLogic _logic;
        [TestInitialize]
        public void Setup()
        {
            Byte.Core.MSTest.Program.Setup();
            _logic = ServiceLocator.Resolve<DataTableLogic>();
        }

        //[TestMethod()]
        //public async Task SelectAsyncTest()
        //{
        //    var result = await _logic.GetDataAsync();
        //    Assert.IsTrue(result != null);
        //}


    }
}