namespace LcnCsharp.Common.Thread
{
    public interface IExecute<T>
    {
        T Execute(object obj, object ob, int checkSate,int deplyTime);
    }
}