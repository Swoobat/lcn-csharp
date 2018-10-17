using System.Data;

namespace LcnCsharp.Core.Datasource
{
    public interface ITxcConnection:IDbConnection,ILCNResource
    {
        /// <summary>
        /// 获取被托管的真实db连接器
        /// </summary>
        /// <returns></returns>
        IDbConnection GetRealDbConnection();

        /// <summary>
        /// 获取被托管的真实db事物
        /// </summary>
        /// <returns></returns>
        IDbTransaction GetRealDbTransaction();
    }
}
