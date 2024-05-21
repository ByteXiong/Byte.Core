namespace Byte.Core.SqlSugar.ConfigOptions;

public class DbConnection
{
    public List<ConnectionItem> ConnectionItem { get; set; }
}

public class ConnectionItem
{

   /// <summary>
   /// 数据库标识
   /// </summary>
    public string ConnId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int HitRate { get; set; }
    /// <summary>
    /// 数据库类型
    /// </summary>
    public int DbType { get; set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; }
}
