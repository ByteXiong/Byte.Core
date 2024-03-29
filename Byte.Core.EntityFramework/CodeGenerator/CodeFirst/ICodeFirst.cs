using Byte.Core.Common.IoC;
using Byte.Core.EntityFramework.IDbContext;
using Byte.Core.EntityFramework.Models;

namespace Byte.Core.EntityFramework.CodeGenerator.CodeFirst
{
    public interface ICodeFirst
    {
        void GenerateAll(bool ifExistCovered = false);
        ICodeFirst GenerateSingle<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>;
        ICodeFirst GenerateIRepository<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>;
        ICodeFirst GenerateRepository<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>;
        ICodeFirst GenerateIBusinessLogic<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>;
        ICodeFirst GenerateBusinessLogic<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>;
        ICodeFirst GenerateController<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>;
    }

    public static class CodeFirstExtensions
    {
        public static ICodeFirst CodeFirst(this IDbContextCore dbContext)
        {
            var codeFirst = ServiceLocator.Resolve<ICodeFirst>();
            if (codeFirst == null)
                throw new Exception("请先在Startup.cs文件的ConfigureServices方法中调用UseCodeGenerator方法以注册。");
            return codeFirst;
        }
    }

}