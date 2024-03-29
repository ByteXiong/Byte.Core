using Byte.Core.EntityFramework.Options;
using System.Text.RegularExpressions;

namespace Byte.Core.EntityFramework.CodeGenerator
{

    /// <summary>
    /// 工厂接口转换器 
    /// </summary>
    public class CodeInterfaceGenerator
    {

        private string Delimiter = "\\";//分隔符，默认为windows下的\\分隔符
        public CodeGenerateOption Option { get; }
        /// <summary>
        /// 静态构造函数：从IoC容器读取配置参数，如果读取失败则会抛出ArgumentNullException异常
        /// </summary>

        public CodeInterfaceGenerator()
        {
        }
        public CodeInterfaceGenerator(CodeGenerateOption option)
        {
            Option = option;
            //Options = ServiceLocator.Resolve<IOptions<CodeGenerateOption>>().Value;
            //if (Options == null)
            //{
            //    throw new ArgumentNullException(nameof(Options));
            //}
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var flag = path.IndexOf("/bin");
            if (flag > 0)
                Delimiter = "/";//如果可以取到值，修改分割符
        }

        public string GenerAll()
        {
            //获取文件 
            var content = ReadCs($"User_DB\\User_AccountLogic.cs");
            Regex rg_chk = new Regex(@"^\)(|\s|\r\n|\r\n\s|\n\r|\n\r\s){.*?\}$");
            // 定义一个Regex对象实
            Match mt_chk = rg_chk.Match(content);
            foreach (var item in mt_chk.Groups)
            {
                content = content.Replace(item.ToString(), ");");
            }

            return content;
        }

        /// <summary>
        /// 从代码模板中读取内容
        /// </summary>
        /// <param name="templateName">模板名称，应包括文件扩展名称。比如：template.cs</param>
        /// <returns></returns>
        public string ReadCs(string csName)
        {
            var content = string.Empty;

            var path = $"{Option.OutputPath}\\{Option.BusinessLogicNamespace}\\{csName}";

            using (var reader = new StreamReader(path))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }

    }
}
