namespace Exact.Azure.AI.GPT.Interface
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
    }
}
