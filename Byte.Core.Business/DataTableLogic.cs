﻿using Byte.Core.Common.Extensions;
using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Business
{
    /// <summary>
    ///  数据表
    /// </summary>
    public  class DataTableLogic 
    {
        readonly IUnitOfWork _unitOfWork;
        public DataTableLogic(IUnitOfWork unitOfWork) { 
        }

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <returns></returns>
        public  async Task<System.Data.DataTable> GetDataAsync()
        {
            var sql = "";
            //var sql =
            //        "SELECT TABLE_NAME as TableName," +
            //        " Table_Comment as TableComment" +
            //        " FROM INFORMATION_SCHEMA.TABLES" +
            //        $" where TABLE_SCHEMA = 'Byte.Core_DB2'";
            if ( _unitOfWork.GetDbClient().CurrentConnectionConfig.DbType == DbType.MySql)
            {
             sql = @"select * from (SELECT (case when a.colorder=1 then d.name else '' end) as TableName,
                      (case when a.colorder=1 then isnull(f.value,'') else '' end) as TableComment
                       FROM syscolumns a
                       inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
                       left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0) t
                       where t.TableName!=''";
            }
            var  aa=await  _unitOfWork.GetDbClient().SqlQueryable<System.Data.DataTable>(sql).FirstAsync();
            return aa;
        }

        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<List<TableColumn>> GetTableColumnsAsync()
        {
            var sql = "";
            if (_unitOfWork.GetDbClient().CurrentConnectionConfig.DbType == DbType.MySql)
            {
                sql =
                 "select table_name as TableName,column_name as ColName, " +
                 " column_default as DefaultValue," +
                 " IF(extra = 'auto_increment','TRUE','FALSE') as IsIdentity," +
                 " IF(is_nullable = 'YES','TRUE','FALSE') as IsNullable," +
                 " DATA_TYPE as ColumnType," +
                 " CHARACTER_MAXIMUM_LENGTH as ColumnLength," +
                 " IF(COLUMN_KEY = 'PRI','TRUE','FALSE') as IsPrimaryKey," +
                 " COLUMN_COMMENT as Comments " +
                 $" from information_schema.columns where table_schema = '{_unitOfWork.GetDbClient()}' and table_name in ({ "user".Select(m => $"'{m}'").Join(",")})";
            }
            var aa = await _unitOfWork.GetDbClient().SqlQueryable<TableColumn>(sql).ToListAsync();
            return aa;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public async Task PageAsync() {
            var sql = "select * from user";
            var aa = await _unitOfWork.GetDbClient().SqlQueryable<dynamic>(sql ).ToListAsync();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public async Task<TableColumn> InfoAsync()
        {
            var sql = "select * from user";
            var aa = await _unitOfWork.GetDbClient().SqlQueryable<dynamic>(sql).FirstAsync();
            return aa.FirstOrDefault();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAsync()
        {
            var sql = "select * from user";
            var aa = await _unitOfWork.GetDbClient().SqlQueryable<dynamic>(sql).ToListAsync();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
            var sql = "select * from user";
            var aa = await _unitOfWork.GetDbClient().SqlQueryable<dynamic>(sql).ToListAsync();
        }

    }
}