using Byte.Core.EntityFramework.IDbContext;
using Byte.Core.EntityFramework.Extensions;
using Byte.Core.EntityFramework.Models;
using Byte.Core.EntityFramework.Options;
using Microsoft.Extensions.Options;
using System.Data;

namespace Byte.Core.EntityFramework.CodeGenerator.DbFirst
{
    internal sealed class DbFirst : IDbFirst
    {
        private CodeGenerator Instance { get; set; }

        private IDbContextCore _dbContext;
        public IDbContextCore DbContext
        {
            get => _dbContext;
            set
            {
                _dbContext = value;
                AllTables = _dbContext.GetCurrentDatabaseTableList().ToList();
            }
        }

        private List<DbTable> AllTables { get; set; }

        public DbFirst(IOptions<CodeGenerateOption> option)
        {
            if (option == null) throw new ArgumentNullException(nameof(option));
            Instance = new CodeGenerator(option.Value);
        }

        public void GenerateAll(CodeTemplateEnum[] type, bool ifExistCovered = false)
        {
            Instance.Generate(AllTables, type, ifExistCovered);
        }

        public IDbFirst Generate(Func<DbTable, bool> selector, CodeTemplateEnum[] type, bool ifExistCovered = false)
        {
            if (selector == null)
                selector = m => true;
            Instance.Generate(AllTables.Where(selector).ToList(), type, ifExistCovered);
            return this;
        }

        public IDbFirst GenerateEntity(Func<DbTable, bool> selector, bool ifExistCovered = false)
        {
            var tables = AllTables.Where(selector).ToList();
            foreach (var table in tables)
            {
                var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
                Instance.GenerateModelEntity(table, pkTypeName, ifExistCovered);
            }
            return this;
        }

        public IDbFirst GenerateSingle(Func<DbTable, bool> selector, bool ifExistCovered = false)
        {
            GenerateEntity(selector, ifExistCovered);
            GenerateIRepository(selector, ifExistCovered);
            GenerateRepository(selector, ifExistCovered);
            GenerateIBusinessLogic(selector, ifExistCovered);
            GenerateBusinessLogic(selector, ifExistCovered);
            GenerateController(selector, ifExistCovered);

            return this;
        }

        public IDbFirst GenerateIRepository(Func<DbTable, bool> selector, bool ifExistCovered = false)
        {
            var tables = AllTables.Where(selector).ToList();
            foreach (var table in tables)
            {
                if (table.Columns.Any(c => c.IsPrimaryKey))
                {
                    var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
                    Instance.GenerateIRepository(table, pkTypeName, ifExistCovered);
                }
            }

            return this;
        }

        public IDbFirst GenerateRepository(Func<DbTable, bool> selector, bool ifExistCovered = false)
        {
            var tables = AllTables.Where(selector).ToList();
            foreach (var table in tables)
            {
                if (table.Columns.Any(c => c.IsPrimaryKey))
                {
                    var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
                    Instance.GenerateRepository(table, pkTypeName, ifExistCovered);
                }
            }

            return this;
        }

        public IDbFirst GenerateIBusinessLogic(Func<DbTable, bool> selector, bool ifExistCovered = false)
        {
            var tables = AllTables.Where(selector).ToList();
            foreach (var table in tables)
            {
                if (table.Columns.Any(c => c.IsPrimaryKey))
                {
                    var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
                    Instance.GenerateIBusinessLogic(table, pkTypeName, ifExistCovered);
                }
            }

            return this;
        }

        public IDbFirst GenerateBusinessLogic(Func<DbTable, bool> selector, bool ifExistCovered = false)
        {
            var tables = AllTables.Where(selector).ToList();
            foreach (var table in tables)
            {
                if (table.Columns.Any(c => c.IsPrimaryKey))
                {
                    var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
                    Instance.GenerateBusinessLogic(table, pkTypeName, ifExistCovered);
                }
            }

            return this;
        }

        public IDbFirst GenerateController(Func<DbTable, bool> selector, bool ifExistCovered = false)
        {
            var tables = AllTables.Where(selector).ToList();
            foreach (var table in tables)
            {
                if (table.Columns.Any(c => c.IsPrimaryKey))
                {
                    var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
                    Instance.GenerateController(table, pkTypeName, ifExistCovered);
                }
            }

            return this;
        }





    }
}
