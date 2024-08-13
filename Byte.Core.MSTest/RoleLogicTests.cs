using Byte.Core.Common.IoC;

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