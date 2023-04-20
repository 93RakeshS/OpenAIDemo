using ExactAzureAIGPT.Factory;

namespace ExactAzureAIGPT.Interface
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(string loggertype);
    }
}
