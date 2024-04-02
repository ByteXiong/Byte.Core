using Byte.Core.Common.CodeGenerator.CodeFirst;
using Byte.Core.Common.CodeGenerator.DbFirst;
using Byte.Core.Common.IDbContext;
using Byte.Core.Common.Models;
using Byte.Core.Common.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Byte.Core.Common.CodeGenerator
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
