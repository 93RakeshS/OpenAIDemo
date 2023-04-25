using ExactAzureAIGPT.Services;
using ILogger = ExactAzureAIGPT.Interface.ILogger;
using ILoggerFactory = ExactAzureAIGPT.Interface.ILoggerFactory;
namespace ExactAzureAIGPT.Factory
{
    
    public class LoggerFactory : ILoggerFactory
    {
        private readonly IConfiguration _configuration;

        public LoggerFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ILogger CreateLogger(string loggertype)
        {
            
            switch (loggertype.ToLowerInvariant())
            {                
                case "text":
                    string logDirectory = _configuration.GetValue<string>("LogFilePath");
                    if (Directory.Exists(logDirectory))
                    {
                        return new FileLogger(logDirectory);
                    }
                    else
                    {
                        Directory.CreateDirectory(logDirectory);
                        return new FileLogger(logDirectory);
                    }

                default:
                    throw new NotSupportedException($"Logging provider type {loggertype} is not supported.");
            }
        }
    }

}
