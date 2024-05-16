namespace Byte.Core.SqlSugar.ConfigOptions;

/// <summary>
/// 全局配置类
/// </summary>
public class Configs
{

    #region 是否开发模式

    private bool? _isQuickDebug;

    /// <summary>
    /// 是否开发模式，生产环境建议设置为False
    /// </summary>
    public bool IsQuickDebug
    {
        get => _isQuickDebug ?? false;
        set => _isQuickDebug = value;
    }

    #endregion
    #region 是否开启读写分离

    private bool? _isCqrs;

    /// <summary>
    /// 是否开发模式
    /// </summary>
    public bool IsCqrs
    {
        get => _isCqrs ?? false;
        set => _isCqrs = value;
    }

    #endregion

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
