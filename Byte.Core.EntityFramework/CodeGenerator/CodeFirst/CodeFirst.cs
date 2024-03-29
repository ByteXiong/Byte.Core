using Byte.Core.EntityFramework.Models;
using Byte.Core.EntityFramework.Options;
using Microsoft.Extensions.Options;

namespace Byte.Core.EntityFramework.CodeGenerator.CodeFirst
{
    internal sealed class CodeFirst : ICodeFirst
    {
        private CodeGenerator Instance { get; set; }
        public CodeFirst(IOptions<CodeGenerateOption> option)
        {
            if (option == null) throw new ArgumentNullException(nameof(option));
            Instance = new CodeGenerator(option.Value);
        }

        public void GenerateAll(bool ifExistCovered = false)
        {
            Instance.Generate(ifExistCovered);
        }

        public ICodeFirst GenerateSingle<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>
        {
            Instance.GenerateSingle<T, TKey>(ifExistCovered);
            return this;
        }

        public ICodeFirst GenerateIRepository<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>
        {
            Instance.GenerateIRepository<T, TKey>(ifExistCovered);
            return this;
        }

        public ICodeFirst GenerateRepository<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>
        {
            Instance.GenerateRepository<T, TKey>(ifExistCovered);
            return this;
        }

        public ICodeFirst GenerateIBusinessLogic<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>
        {
            Instance.GenerateIBusinessLogic<T, TKey>(ifExistCovered);
            return this;
        }

        public ICodeFirst GenerateBusinessLogic<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>
        {
            Instance.GenerateBusinessLogic<T, TKey>(ifExistCovered);
            return this;
        }

        public ICodeFirst GenerateController<T, TKey>(bool ifExistCovered = false) where T : IBaseModel<TKey>
        {
            Instance.GenerateController<T, TKey>(ifExistCovered);
            return this;
        }
    }
}
