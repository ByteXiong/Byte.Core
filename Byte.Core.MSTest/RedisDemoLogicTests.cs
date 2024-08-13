using Byte.Core.Common.Extensions;
using Byte.Core.Common.IoC;

namespace Byte.Core.Business.Tests
{
    [TestClass()]
    public class RedisDemoLogicTests
    {

        public RedisDemoLogic _logic;
        [TestInitialize]
        public void Setup()
        {
            Byte.Core.MSTest.Program.Setup();
            _logic = ServiceLocator.Resolve<RedisDemoLogic>();
        }

        [TestMethod()]
        public async Task GetByIdsTest()
        {

            //_roleLogic.AddInterceptor("DoSomething", new MyInterceptor());

            // 调用DoSomething方法
            
        var result = await _logic.GetByIdAsync("DF6CA8BA-F204-4622-BD3F-000061E4E567".ToGuid().Value);
            //var result = await _logic.GetByIdsAsync(new Guid[] { Guid.NewGuid(), Guid.NewGuid() });

            Assert.IsTrue(result != null);
        }
    }
}