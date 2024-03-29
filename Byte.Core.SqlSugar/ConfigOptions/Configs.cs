using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.SqlSugar.ConfigOptions;

/// <summary>
/// 全局配置类
/// </summary>
public class Configs
{
  

    #region 数据库连接对象

    private DbConnection _dataConnection;

    /// <summary>
    ///  数据库连接对象
    /// </summary>
    public DbConnection DataConnection
    {
        get
        {
            if (_dataConnection == null)
            {
                _dataConnection = new DbConnection
                {
                    ConnectionItem = new List<ConnectionItem>()
                };
            }

            return _dataConnection;
        }
        set => _dataConnection = value;
    }
    #endregion
}
