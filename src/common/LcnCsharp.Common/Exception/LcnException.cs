namespace LcnCsharp.Common.Exception
{
    public class LcnException : System.Exception
    {
        public LcnException(string message) : base(message)
        {

        }

        public LcnException(string cause, System.Exception throwable) : base(cause, throwable)
        {
        }
    }
}
