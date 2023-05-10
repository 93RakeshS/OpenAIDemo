using Exact.Azure.AI.GPT.Factory;

namespace Exact.Azure.AI.GPT.Interface
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(string loggertype);
    }
}
