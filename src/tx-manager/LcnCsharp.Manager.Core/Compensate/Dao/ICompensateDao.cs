using System.Collections.Generic;
using LcnCsharp.Manager.Core.Compensate.Model;

namespace LcnCsharp.Manager.Core.Compensate.Dao
{
    public interface ICompensateDao
    {
        string SaveCompensateMsg(TransactionCompensateMsg transactionCompensateMsg);

        List<string> LoadCompensateKeys();

        List<string> LoadCompensateTimes(string model);

        List<string> LoadCompensateByModelAndTime(string path);

        string GetCompensate(string key);

        string GetCompensateByGroupId(string groupId);

        void DeleteCompensateByPath(string path);

        void DeleteCompensateByKey(string key);

        bool HasCompensate();
    }
}