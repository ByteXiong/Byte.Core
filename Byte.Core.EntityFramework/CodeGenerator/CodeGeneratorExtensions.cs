using Byte.Core.EntityFramework.CodeGenerator.CodeFirst;
using Byte.Core.EntityFramework.CodeGenerator.DbFirst;
using Byte.Core.EntityFramework.IDbContext;
using Byte.Core.EntityFramework.Models;
using Byte.Core.EntityFramework.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Byte.Core.EntityFramework.CodeGenerator
{
    public static class CodeGeneratorExtenstions
    {


        public static void GenerateAllCodesFromDatabase(this IDbContextCore dbContext, CodeTemplateEnum[] type, bool ifExistCovered = false,
            Func<DbTable, bool> selector = null)
        {
            dbContext.DbFirst().Generate(selector, type, ifExistCovered);
        }

        public static void UseCodeGenerator(this IServiceCollection services, CodeGenerateOption option)
        {
            if (option == null)
                throw new ArgumentNullException(nameof(option));
            services.Configure<CodeGenerateOption>(config =>
           {
               config.ControllersNamespace = option.ControllersNamespace;
               config.IRepositoriesNamespace = option.IRepositoriesNamespace;
               config.IBusinessLogicNamespace = option.IBusinessLogicNamespace;
               config.ModelsNamespace = option.ModelsNamespace;
               config.OutputPath = option.OutputPath;
               config.RepositoriesNamespace = option.RepositoriesNamespace;
               config.BusinessLogicNamespace = option.BusinessLogicNamespace;
               config.IsPascalCase = option.IsPascalCase;

               config.ModelParamNamespace = option.ModelParamNamespace;
               config.ModelRequestNamespace = option.ModelRequestNamespace;
           });
            services.AddSingleton<IDbFirst, DbFirst.DbFirst>();
            services.AddSingleton<ICodeFirst, CodeFirst.CodeFirst>();
        }
    }
}
