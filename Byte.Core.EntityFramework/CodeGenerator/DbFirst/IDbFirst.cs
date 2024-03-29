using Byte.Core.Common.IoC;
using Byte.Core.EntityFramework.IDbContext;
using Byte.Core.EntityFramework.Models;

namespace Byte.Core.EntityFramework.CodeGenerator.DbFirst
{
    public interface IDbFirst
    {
        IDbContextCore DbContext { get; set; }
        void GenerateAll(CodeTemplateEnum[] type, bool ifExistCovered = false);
        IDbFirst Generate(Func<DbTable, bool> selector, CodeTemplateEnum[] type, bool ifExistCovered = false);
        IDbFirst GenerateEntity(Func<DbTable, bool> selector, bool ifExistCovered = false);
        IDbFirst GenerateSingle(Func<DbTable, bool> selector, bool ifExistCovered = false);
        IDbFirst GenerateIRepository(Func<DbTable, bool> selector, bool ifExistCovered = false);
        IDbFirst GenerateRepository(Func<DbTable, bool> selector, bool ifExistCovered = false);
        IDbFirst GenerateIBusinessLogic(Func<DbTable, bool> selector, bool ifExistCovered = false);
        IDbFirst GenerateBusinessLogic(Func<DbTable, bool> selector, bool ifExistCovered = false);
        IDbFirst GenerateController(Func<DbTable, bool> selector, bool ifExistCovered = false);

    }

    public static class DbFirstExtensions
    {
        public static IDbFirst DbFirst(this IDbContextCore dbContext)
        {
            var dbFirst = ServiceLocator.Resolve<IDbFirst>();
            if (dbFirst == null)
                throw new Exception("请先在Startup.cs文件的ConfigureServices方法中调用UseCodeGenerator方法以注册。");
            dbFirst.DbContext = dbContext;
            return dbFirst;
        }
    }
}