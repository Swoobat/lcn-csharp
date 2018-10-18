namespace LcnCsharp.Manager.Core.Netty.Service
{
    public interface INettyService
    {
        IActionService GetActionService(string action);
    }
}