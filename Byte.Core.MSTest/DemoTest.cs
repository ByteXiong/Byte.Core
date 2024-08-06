using Microsoft.VisualStudio.TestTools.UnitTesting;
using Byte.Core.Business;
using Byte.Core.SqlSugar;
using Byte.Core.Common.IoC;

namespace Byte.Core.Business.Tests
{
    [TestClass()]
    public class DemoTest
    {
        public IUnitOfWork  _unitOfWork;
        [TestInitialize]
        public void Setup()
        {
            Byte.Core.MSTest.Program.Setup();
            _unitOfWork = ServiceLocator.Resolve<IUnitOfWork>();
        }
       

    
    }
}