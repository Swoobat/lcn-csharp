using System;
using System.Collections.Generic;
using LcnCsharp.Manager.Core.Compensate.Model;
using LcnCsharp.Manager.Core.Model;
using LcnCsharp.Manager.Core.Netty.Model;

namespace LcnCsharp.Manager.Core.Compensate.Service
{
    public interface ICompensateService
    {
        bool SaveCompensateMsg(TransactionCompensateMsg transactionCompensateMsg);

        List<ModelName> LoadModelList();

        List<String> LoadCompensateTimes(string model);

        List<TxModel> LoadCompensateByModelAndTime(string path);

        void AutoCompensate(string compensateKey, TransactionCompensateMsg transactionCompensateMsg);

        bool ExecuteCompensate(string key);

        void ReloadCompensate(TxGroup txGroup);

        bool HasCompensate();

        bool DelCompensate(string path);

        TxGroup GetCompensateByGroupId(string groupId);
    }
}