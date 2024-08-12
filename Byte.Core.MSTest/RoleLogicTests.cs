using Microsoft.VisualStudio.TestTools.UnitTesting;
using Byte.Core.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Byte.Core.SqlSugar;
using Byte.Core.Common.IoC;
using Byte.Core.Common.Attributes.RedisAttribute;

namespace Byte.Core.Business.Tests
{
    [TestClass()]
    public class RoleLogicTests
    {
        public RoleLogic _roleLogic;
        [TestInitialize]
        public void Setup()
        {
            Byte.Core.MSTest.Program.Setup();
            _roleLogic = ServiceLocator.Resolve<RoleLogic>();
        }

        [TestMethod()]
        public async Task  SelectAsyncTest()
        {

            //_roleLogic.AddInterceptor("DoSomething", new MyInterceptor());

            // 调用DoSomething方法

            var result=    await _roleLogic.SelectAsync("张三");
            
            Assert.IsTrue(result != null);
        }
    }
}