using SqlSugar;

namespace Byte.Core.SqlSugar.Repository;

public interface IUnitOfWork
{
    SqlSugarScope GetDbClient();
    void BeginTran();
    void CommitTran();
    void RollbackTran();
}
