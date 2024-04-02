namespace Byte.Core.Common.CodeGenerator
{
    public class CodeGenerateOption
    {
        public virtual string OutputPath { get; set; }
        public virtual string CommonNamespace { get; set; }
        public virtual string ModelsNamespace { get; set; }

        public virtual string ControllersNamespace { get; set; }
        public virtual string IRepositoriesNamespace { get; set; }
        public virtual string RepositoriesNamespace { get; set; }
        public virtual string IBusinessLogicNamespace { get; set; }
        public virtual string BusinessLogicNamespace { get; set; }
        public bool IsPascalCase { get; set; } = true;
        public bool GenerateApiController { get; set; } = true;//是否生成Api

        public virtual string ModelParamNamespace { get; set; }
        public virtual string ModelRequestNamespace { get; set; }
        public virtual string UINamespace { get; set; }
    }
}
