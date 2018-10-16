using System.Data;

namespace LcnCsharp.Core.datasource
{
    public interface ITxcConnection:IDbConnection,ILCNResource
    {
        //void Commit(IDbTransaction dbTransaction);
        //void Rollback(IDbTransaction dbTransaction);
        //void Close(IDbConnection dbConnection);
    }
}
