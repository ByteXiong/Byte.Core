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
    #region 是否初始DataTable

    private bool? _isInitTable;

    /// <summary>
    /// 是否初始DataTable
    /// </summary>
    public bool IsInitTable
    {
        get => _isInitTable ?? false;
        set => _isInitTable = value;
    }

    #endregion


    #region 是否初始数据

    private bool? _isInitData;

    /// <summary>
    /// 是否初始Data
    /// </summary>
    public bool IsInitData
    {
        get => _isInitData ?? false;
        set => _isInitData = value;
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

    #region 默认DB

    private string _defaultDataBase;

    /// <summary>
    /// 默认DB
    /// </summary>
    public string DefaultDataBase
    {
        get => _defaultDataBase ?? DataConnection.ConnectionItem.FirstOrDefault().ConnId;
        set => _defaultDataBase = value;
    }

    #endregion

    #region 日志DB

    private string _logDataBase;

    /// <summary>
    /// 默认DB
    /// </summary>
    public string LogDataBase { get; set; }

    #endregion

    #region 数据库连接对象

    private DbConnection _dbConnection;

    /// <summary>
    ///  数据库连接对象
    /// </summary>
    public DbConnection DataConnection
    {
        get
        {
            if (_dbConnection == null)
            {
                _dbConnection = new DbConnection
                {
                    ConnectionItem = new List<ConnectionItem>()
                };
            }
            return _dbConnection;
        }
        set => _dbConnection = value;
    }
    #endregion
}
