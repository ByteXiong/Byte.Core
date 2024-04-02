using Byte.Core.Common.Extensions;
using System.Data;
using System.Reflection;
using System.Text;

namespace Byte.Core.Common.CodeGenerator
{
    /// <summary>
    /// 代码生成器。
    /// <remarks>
    /// 根据指定的实体域名空间生成Repositories和Services层的基础代码文件。
    /// </remarks>
    /// </summary>
    public class CodeGenerator
    {
        private string Delimiter = "\\";//分隔符，默认为windows下的\\分隔符
        public CodeGenerateOption Option { get; }
        /// <summary>
        /// 静态构造函数：从IoC容器读取配置参数，如果读取失败则会抛出ArgumentNullException异常
        /// </summary>

        public CodeGenerator()
        {
        }
        public CodeGenerator(CodeGenerateOption option)
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

        /// <summary>
        /// 生成指定的实体域名空间下各实体对应Repositories和Services层的基础代码文件
        /// </summary>
        /// <param name="ifExistCovered">如果目标文件存在，是否覆盖。默认为false</param>
        public void Generate(bool ifExistCovered = false)
        {
            var assembly = Assembly.Load(Option.ModelsNamespace);
            var types = assembly?.GetTypes();
            var list = types;
            if (list != null)
            {
                foreach (var type in list)
                {
                    GenerateSingle(type, ifExistCovered);
                }
            }
        }

    

        public void GenerateController<TEntity>(bool ifExistCovered = false) where TEntity:class
        {
            var db = new DbTable();
            db.TableName = typeof(TEntity).Name;
            GenerateController(db, ifExistCovered);

        }
        /// <summary>
        /// 生成代码模板
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="type"></param>
        /// <param name="ifExistCovered"></param>
        /// <exception cref="Exception"></exception>
        public void Generate(List<DbTable> tables, CodeTemplateEnum[] type, bool ifExistCovered = false)
        {
            foreach (var table in tables)
            {
                if (table.Columns.Any(c => c.IsPrimaryKey))
                {
                    var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
                    foreach (var item in type)
                    {
                        switch (item)
                        {
                            case CodeTemplateEnum.Controllers:
                                if (!Option.ControllersNamespace.IsNullOrWhiteSpace())
                                    GenerateController(table, ifExistCovered);

                                break;
                            case CodeTemplateEnum.BusinessLogic:
                                if (!Option.BusinessLogicNamespace.IsNullOrWhiteSpace())
                                    GenerateBusinessLogic(table,ifExistCovered);
                                break;
                            case CodeTemplateEnum.IBusinessLogic:

                                if (!Option.IBusinessLogicNamespace.IsNullOrWhiteSpace())
                                    GenerateIBusinessLogic(table, ifExistCovered);
                                break;

                            case CodeTemplateEnum.Repositories:
                                if (!Option.RepositoriesNamespace.IsNullOrWhiteSpace())
                                    GenerateRepository(table, ifExistCovered);
                                break;
                            case CodeTemplateEnum.IRepositories:


                                if (!Option.IRepositoriesNamespace.IsNullOrWhiteSpace())
                                    GenerateIRepository(table,  ifExistCovered);
                                break;

                            case CodeTemplateEnum.Entity:
                                if (!Option.ModelsNamespace.IsNullOrWhiteSpace())
                                    GenerateModelEntity(table,  ifExistCovered);
                                break;
                            case CodeTemplateEnum.Models:
                                if (!Option.ModelParamNamespace.IsNullOrWhiteSpace())
                                    GenerateModelParam(table, ifExistCovered);
                                if (!Option.ModelRequestNamespace.IsNullOrWhiteSpace())
                                    GenerateModelRequest(table, ifExistCovered);

                                break;
                            case CodeTemplateEnum.UI:
                                if (!Option.UINamespace.IsNullOrWhiteSpace())
                                {
                                    GenerateUIList(table, ifExistCovered);
                                    GenerateUIEditForm(table, ifExistCovered);
                                }
                                break;
                            default:
                                break;
                        }


                    }


                }
                else
                {
                    throw new Exception(table.TableName + "不存在主键关系");
                }
            }
        }
        /// <summary>
        /// 生成指定的实体对应IBusinessLogics和BusinessLogic层的基础代码文件
        /// </summary>
        /// <param name="modelType">实体类型（必须实现IBaseModel接口）</param>
        /// <param name="ifExistCovered">如果目标文件存在，是否覆盖。默认为false</param>
        public void GenerateSingle(Type modelType, bool ifExistCovered = false)
        {
            var db = new DbTable();
            db.TableName = modelType.Name;
            var keyTypeName = modelType.GetProperty("Id")?.PropertyType.Name;
            GenerateIRepository(db, ifExistCovered);
            GenerateRepository(db, ifExistCovered);
            GenerateIBusinessLogic(db, ifExistCovered);
            GenerateBusinessLogic(db, ifExistCovered);
            GenerateController(db, ifExistCovered);
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public void WriteAndSave(string fileName, string content)
        {
            //实例化一个文件流--->与写入文件相关联
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                //实例化一个StreamWriter-->与fs相关联
                using (var sw = new StreamWriter(fs))
                {
                    //开始写入
                    sw.Write(content);
                    //清空缓冲区
                    sw.Flush();
                    //关闭流
                    sw.Close();
                    fs.Close();
                }
            }
        }



        #region 生成代码层


        /// <summary>
        /// 从代码模板中读取内容
        /// </summary>
        /// <param name="templateName">模板名称，应包括文件扩展名称。比如：template.txt</param>
        /// <returns></returns>
        public string ReadTemplate(string templateName)
        {
            var content = string.Empty;

            var path = $"{Option.OutputPath}\\{Option.CommonNamespace}\\CodeTemplate\\{templateName}";

            using (var reader = new StreamReader(path))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }
        /// <summary>
        /// 替换模板内容
        /// </summary>
        /// <param name="content"></param>
        /// <param name="table"></param>
        /// <param name="pkTypeName"></param>
        /// <returns></returns>
        public string ReplaceTemplate(string content, DbTable table)
        {

            content = content
                .Replace("{ModelsNamespace}", Option.ModelsNamespace)
                .Replace("{CommonNamespace}", Option.CommonNamespace)
                .Replace("{IRepositoriesNamespace}", Option.IRepositoriesNamespace)
                .Replace("{RepositoriesNamespace}", Option.RepositoriesNamespace)
                .Replace("{IBusinessLogicNamespace}", Option.IBusinessLogicNamespace)
                .Replace("{BusinessLogicNamespace}", Option.ControllersNamespace)
                .Replace("{ModelParamNamespace}", Option.ModelParamNamespace)
                .Replace("{ModelRequestNamespace}", Option.ModelRequestNamespace)
                .Replace("{ControllersNamespace}", Option.ControllersNamespace)
                .Replace("{TableName}", table.TableName)//表名
                .Replace("{Comment}", table.TableComment)//注释
                .Replace("{ModelString}", GenerateEntityString(table));
            return content;
        }



        public string ReadFile(string fileFullName)
        {
            return File.ReadAllText(fileFullName);
        }

        public string ReadFile(string fileType, string fileName)
        {
            var fileFullName = Option.OutputPath + Delimiter + fileType + Delimiter + fileName;
            return ReadFile(fileFullName);
        }

        /// <summary>
        /// 生成数据库对象
        /// </summary>
        /// <param name="table"></param>
        /// <param name="ifExistCovered"></param>

        public void GenerateModelEntity(DbTable table, bool ifExistCovered = false)
        {
            var aa = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            var modelPath = Option.OutputPath + Delimiter + Option.ModelsNamespace;
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }
            var fullPath = modelPath + Delimiter + table.TableName + ".cs";
            if (File.Exists(fullPath) && !ifExistCovered)
                return;
            var content = ReadTemplate("ModelTemplate.txt");
            content = ReplaceTemplate(content, table);
            content = content.Replace("{ModelProperties}", GenerateEntity(table));
            WriteAndSave(fullPath, content);
        }
        public void GenerateModelRequest(DbTable table, bool ifExistCovered = false)
        {
            var modelPath = Option.OutputPath + Delimiter + Option.ModelRequestNamespace;
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }
            var fullPath = modelPath + Delimiter + table.TableName + "DTO.cs";
            if (File.Exists(fullPath) && !ifExistCovered)
                return;
            var content = ReadTemplate("ModelRequestTemplate.txt");
            table.Columns = table.Columns.Where(x => !x.IsPrimaryKey).ToList();
            content = ReplaceTemplate(content, table);
            content = content.Replace("{ModelProperties}", GenerateModel(table))
                        .Replace("{ModelRequestNamespace}", Option.ModelRequestNamespace)
                ;
            WriteAndSave(fullPath, content);
        }

        /// <summary>
        /// 接口传参
        /// </summary>
        /// <param name="table"></param>
        /// <param name="pkTypeName"></param>
        /// <param name="ifExistCovered"></param>
        public void GenerateModelParam(DbTable table, bool ifExistCovered = false)
        {
            var modelPath = Option.OutputPath + Delimiter + Option.ModelParamNamespace;
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }
            var fullPath = modelPath + Delimiter + table.TableName + "Param.cs";
            if (File.Exists(fullPath) && !ifExistCovered)
                return;
            var content = ReadTemplate("ModelParamTemplate.txt");
            table.Columns = table.Columns.Where(x => !x.IsPrimaryKey).ToList();
            content = ReplaceTemplate(content, table);
            content = content.Replace("{ModelProperties}", GenerateModel(table))
                  .Replace("{ModelParamNamespace}", Option.ModelParamNamespace)
                ;
            WriteAndSave(fullPath, content);
        }






        /// <summary>
        /// 生成IRepository层代码文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="pkTypeName"></param>
        /// <param name="ifExistCovered"></param>
        public void GenerateIRepository(DbTable table, bool ifExistCovered = false)
        {
            var iRepositoryPath = Option.OutputPath + Delimiter + Option.IRepositoriesNamespace;
            if (!Directory.Exists(iRepositoryPath))
            {
                Directory.CreateDirectory(iRepositoryPath);
            }
            var fullPath = iRepositoryPath + Delimiter + "I" + table.TableName + "Repository.cs";
            if (File.Exists(fullPath) && !ifExistCovered)
                return;
            var content = ReadTemplate("IRepositoryTemplate.txt");
            content = ReplaceTemplate(content, table);
            WriteAndSave(fullPath, content);
        }

        public void GenerateIRepository<TEntity>(bool ifExistCovered = false) where TEntity:class
        {
            var db = new DbTable();
            db.TableName = typeof(TEntity).Name;
            GenerateIRepository(db, ifExistCovered);

        }
        /// <summary>
        /// 生成Repository层代码文件
        /// </summary>
        /// <param name="modelTypeName"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExistCovered"></param>
        public void GenerateRepository(DbTable table, bool ifExistCovered = false)
        {
            var repositoryPath = Option.OutputPath + Delimiter + Option.RepositoriesNamespace;
            if (!Directory.Exists(repositoryPath))
            {
                Directory.CreateDirectory(repositoryPath);
            }
            var fullPath = repositoryPath + Delimiter + table.TableName + "Repository.cs";
            if (File.Exists(fullPath) && !ifExistCovered)
                return;
            var content = ReadTemplate("RepositoryTemplate.txt");
            content = ReplaceTemplate(content, table);

            WriteAndSave(fullPath, content);
        }
        public void GenerateRepository<TEntity>(bool ifExistCovered = false) where TEntity:class
        {
            var db = new DbTable();
            db.TableName = typeof(TEntity).Name;
            GenerateRepository(db, ifExistCovered);

        }
        /// <summary>
        /// 生成IBusinessLogic层代码文件
        /// </summary>
        /// <param name="modelTypeName"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExistCovered"></param>
        public void GenerateIBusinessLogic(DbTable table, bool ifExistCovered = false)
        {
            var iRepositoryPath = Option.OutputPath + Delimiter + Option.IBusinessLogicNamespace;
            if (!Directory.Exists(iRepositoryPath))
            {
                Directory.CreateDirectory(iRepositoryPath);
            }
            var fullPath = iRepositoryPath + Delimiter + "I" + table.TableName + "Logic.cs";
            if (File.Exists(fullPath) && !ifExistCovered)
                return;
            var content = ReadTemplate("IBusinessLogicTemplate.txt");
            content = ReplaceTemplate(content, table);
            WriteAndSave(fullPath, content);
        }

        public void GenerateIBusinessLogic<TEntity>(bool ifExistCovered = false) where TEntity:class
        {
            var db = new DbTable();
            db.TableName = typeof(TEntity).Name;
            GenerateIBusinessLogic(db, ifExistCovered);
        }
        /// <summary>
        /// 生成BusinessLogic层代码文件
        /// </summary>
        /// <param name="modelTypeName"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExistCovered"></param>
        public void GenerateBusinessLogic(DbTable table, bool ifExistCovered = false)
        {
            var repositoryPath = Option.OutputPath + Delimiter + Option.BusinessLogicNamespace;
            if (!Directory.Exists(repositoryPath))
            {
                Directory.CreateDirectory(repositoryPath);
            }
            var fullPath = repositoryPath + Delimiter + table.TableName + "Logic.cs";
            if (File.Exists(fullPath) && !ifExistCovered)
                return;
            var content = ReadTemplate("BusinessLogicTemplate.txt");
            content = ReplaceTemplate(content, table);
            WriteAndSave(fullPath, content);
        }
        public void GenerateBusinessLogic<TEntity>(bool ifExistCovered = false) where TEntity: class
        {
            var db = new DbTable();
            db.TableName = typeof(TEntity).Name;
            GenerateBusinessLogic(db, ifExistCovered);

        }

        /// <summary>
        /// 生成Controller层代码文件
        /// </summary>
        /// <param name="modelTypeName"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExistCovered"></param>
        public void GenerateController(DbTable table, bool ifExistCovered = false)
        {
            var controllerPath = Option.OutputPath + Delimiter + Option.ControllersNamespace + Delimiter + "Controllers";
            if (!Directory.Exists(controllerPath))
            {
                Directory.CreateDirectory(controllerPath);
            }
            var fullPath = controllerPath + Delimiter + table.TableName + "Controller.cs";
            if (File.Exists(fullPath) && !ifExistCovered)
                return;
            var content = ReadTemplate(Option.GenerateApiController ? "ApiControllerTemplate.txt" : "ControllerTemplate.txt");
            content = ReplaceTemplate(content, table);
            // content = content.Replace("{ModelsNamespace}", Option.ModelsNamespace)
            //.Replace("{IBusinessLogicNamespace}", Option.IBusinessLogicNamespace)
            //.Replace("{ControllersNamespace}", Option.ControllersNamespace)
            //.Replace("{ModelTypeName}", modelTypeName)
            //.Replace("{KeyTypeName}", keyTypeName)
            //.Replace("{CommonNamespace}", Option.CommonNamespace);
            WriteAndSave(fullPath, content);
        }


        /// <summary>
        /// 生成UI List页面
        /// </summary>
        /// <param name="modelTypeName"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExistCovered"></param>
        public void GenerateUIList(DbTable table, bool ifExistCovered = false)
        {
            var repositoryPath = Option.OutputPath + Delimiter + Option.UINamespace + Delimiter + table.TableName.ToFirstLowerStr();//转小写  
            if (!Directory.Exists(repositoryPath))
            {
                Directory.CreateDirectory(repositoryPath);
            }
            var fullPath = repositoryPath + Delimiter + "list.vue"; ;
            if (File.Exists(fullPath) && !ifExistCovered)
                return;
            var content = ReadTemplate("ListTemplate.txt");
            content = ReplaceTemplate(content, table);
            var sb = new StringBuilder();
            foreach (var column in table.Columns.Where(x => !x.IsPrimaryKey))
            {
                var tmp = GenerateListString(column);
                sb.AppendLine(tmp);
            }

            content = content.Replace("{TableColumnNamespace}", sb.ToString());

            WriteAndSave(fullPath, content);
        }

        /// <summary>
        /// 生成UI EditForm页面
        /// </summary>
        /// <param name="modelTypeName"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExistCovered"></param>
        public void GenerateUIEditForm(DbTable table, bool ifExistCovered = false)
        {
            var repositoryPath = Option.OutputPath + Delimiter + Option.UINamespace + Delimiter + table.TableName.ToFirstLowerStr();//转小写  
            if (!Directory.Exists(repositoryPath))
            {
                Directory.CreateDirectory(repositoryPath);
            }
            var fullPath = repositoryPath + Delimiter + "editForm.vue"; ;
            if (File.Exists(fullPath) && !ifExistCovered)
                return;
            var content = ReadTemplate("EditFormTemplate.txt");
            content = ReplaceTemplate(content, table);

            var sb = new StringBuilder();
            foreach (var column in table.Columns.Where(x => !x.IsPrimaryKey))
            {
                var tmp = GenerateEditFormString(column);
                sb.AppendLine(tmp);
            }

            content = content.Replace("{FormNamespace}", sb.ToString());
            WriteAndSave(fullPath, content);
        }



        #endregion





        #region 生成对象 

        public string GenerateEntity(DbTable table)
        {
            var sb = new StringBuilder();
            foreach (var column in table.Columns)
            {
                var tmp = GenerateEntityProperty(column);
                sb.AppendLine(tmp);
                sb.AppendLine();
            }
            return sb.ToString();
        }
        public string GenerateEntityString(DbTable table)
        {
            var sb = new StringBuilder();
            foreach (var column in table.Columns.Where(x => !x.IsPrimaryKey))
            {
                var tmp = GenerateEntityString(column);
                sb.AppendLine(tmp);
            }
            return sb.ToString();
        }




        public string GenerateModel(DbTable table)
        {
            var sb = new StringBuilder();
            foreach (var column in table.Columns)
            {
                var tmp = GenerateModelProperty(column);
                sb.AppendLine(tmp);
                sb.AppendLine();
            }
            return sb.ToString();
        }



        /// <summary>
        /// 生成实体对象
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GenerateEntityProperty(DbTableColumn column)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(column.Comments))
            {
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendLine("\t\t/// " + column.Comments);
                sb.AppendLine("\t\t/// </summary>");
            }
            if (column.IsPrimaryKey)
            {
                sb.AppendLine("\t\t[Key]");
                sb.AppendLine($"\t\t[Column(\"{column.ColName}\")]");
                if (column.IsIdentity)
                {
                    sb.AppendLine("\t\t[DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
                }
                sb.AppendLine($"\t\tpublic override {column.CSharpType} Id " + "{get;set;}");
            }
            else
            {
                if (Option.IsPascalCase)
                {
                    sb.AppendLine($"\t\t[Column(\"{column.ColName}\")]");
                }
                if (!column.IsNullable)
                {
                    sb.AppendLine("\t\t[Required]");
                }

                var colType = column.CSharpType;
                if (colType.ToLower() == "string" && column.ColumnLength.HasValue && column.ColumnLength.Value > 0)
                {
                    sb.AppendLine($"\t\t[MaxLength({column.ColumnLength.Value})]");
                }
                if (column.IsIdentity)
                {
                    sb.AppendLine("\t\t[DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
                }

                if (colType.ToLower() != "string" && colType.ToLower() != "byte[]" && colType.ToLower() != "object" &&
                    column.IsNullable)
                {
                    colType = colType + "?";
                }

                var colName = column.ColName;
                if (!column.Alias.IsNullOrEmpty()) colName = column.Alias;
                if (Option.IsPascalCase) colName = colName.ToPascalCase();
                sb.AppendLine($"\t\tpublic {colType} {colName} " + "{get;set;}");
            }

            return sb.ToString();
        }
        /// <summary>
        /// 生成对象
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GenerateModelProperty(DbTableColumn column)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(column.Comments))
            {
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendLine("\t\t/// " + column.Comments);
                sb.AppendLine("\t\t/// </summary>");
            }

            if (!column.IsNullable)
            {
                sb.AppendLine("\t\t[Required]");
            }

            var colType = column.CSharpType;
            if (colType.ToLower() == "string" && column.ColumnLength.HasValue && column.ColumnLength.Value > 0)
            {
                sb.AppendLine($"\t\t[MaxLength({column.ColumnLength.Value},ErrorMessage=\"超出最大长度\")]");
            }

            if (colType.ToLower() != "string" && colType.ToLower() != "byte[]" && colType.ToLower() != "object" &&
                column.IsNullable)
            {
                colType = colType + "?";
            }

            var colName = column.ColName;
            if (!column.Alias.IsNullOrEmpty()) colName = column.Alias;
            if (Option.IsPascalCase) colName = colName.ToPascalCase();
            sb.AppendLine($"\t\tpublic {colType} {colName} " + "{get;set;}");


            return sb.ToString();
        }

        /// <summary>
        /// 生成工具备注
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GenerateEntityString(DbTableColumn column)
        {
            var sb = new StringBuilder();
            var colName = column.ColName;
            sb.Append($"{colName}= param.{colName}, //{column.Comments}");
            return sb.ToString();
        }


        /// <summary>
        /// 生成UI List 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GenerateListString(DbTableColumn column)
        {
            var sb = new StringBuilder();
            var colName = column.ColName.ToFirstLowerStr();//转小写
            var colType = column.CSharpType;
            //sb.AppendLine("\t\t <el-table-column");
            //sb.AppendLine($"\t\t   prop = \"{colName}\"");
            //sb.AppendLine($"\t\t label = \"{column.Comments}\"");
            //switch (colType)
            //{
            //    case "DateTime":
            //        sb.AppendLine("\t\t  width =\"140\"");
            //        break;
            //    default:
            //        sb.AppendLine("\t\t  min-width =\"80\"");
            //        break;
            //}
            //sb.AppendLine("\t\t   sortable = \"custom\">");
            //sb.AppendLine("\t\t   </el-table-column>");


            sb.AppendLine("{");
                sb.AppendLine($"field: '{colName}',");

                sb.AppendLine($"label: '{column.Comments}'");
                 sb.AppendLine("},");

               return sb.ToString();
        }


        /// <summary>
        /// 生成UI EditForm
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GenerateEditFormString(DbTableColumn column)
        {
            var sb = new StringBuilder();
            var colName = column.ColName.ToFirstLowerStr();//转小写

            var colType = column.CSharpType;

            //sb.AppendLine($"\t\t  <el-form-item label=\"{column.Comments}\" prop=\"{colName}\"       :rules=\"{{ required: true, message: '{column.Comments}不能为空', trigger:['blur', 'change'] }}\"  >");

            //switch (colType)
            //{
            //    case "Int32":
            //        sb.AppendLine($"\t\t <el-input type=\"number\" v-model=\"form.{colName}\"  autocomplete=\"off\" placeholder=\"请输入{column.Comments}\" ></el-input>");
            //        break;
            //    case "DateTime":
            //        sb.AppendLine($"\t\t <el-date-picker v-model=\"form.{colName}\" type = \"datetime\"   placeholder=\"请选择{column.Comments}\" > </el-date-picker > ");
            //        break;
            //    default:
            //        sb.AppendLine($"\t\t  <el-input v-model=\"form.{colName}\" autocomplete=\"off\" placeholder=\"请输入{column.Comments}\"></el-input>");
            //        break;
            //}

            //sb.AppendLine($"\t\t   </el-form-item>");


            sb.AppendLine("{");
            sb.AppendLine($"field: '{colName}',");
            sb.AppendLine($"label: '{column.Comments}',");
            sb.AppendLine("component: 'Input',");
            sb.AppendLine("colProps:");
            sb.AppendLine("{");
            sb.AppendLine("   span: 12");
            sb.AppendLine("},");
            if(colType== "Int32")
                sb.AppendLine(" componentProps: {type: 'number' ,min : 0}");

            if (colType == "String") {

                sb.AppendLine(" componentProps: {max : "+ column.ColumnLength+ "}");
            }



            sb.AppendLine("},"); 

            return sb.ToString();
        }
        //<el-input type = "age" v-model.number="numberValidateForm.age" autocomplete="off"></el-input>
        #endregion


    }
}
