using System.Data;

namespace LcnCsharp.Core.Datasource
{
    public interface ITxcConnection:IDbConnection,ILCNResource
    {
        //void Commit(IDbTransaction dbTransaction);
        //void Rollback(IDbTransaction dbTransaction);
        //void Close(IDbConnection dbConnection);
    }
}
