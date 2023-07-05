using Devon.Azure.AI.GPT.Factory;

namespace Devon.Azure.AI.GPT.Interface
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(string loggertype);
    }
}
