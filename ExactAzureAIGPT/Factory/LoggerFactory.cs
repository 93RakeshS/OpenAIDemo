using Devon.Azure.AI.GPT.Services;
using ILogger = Devon.Azure.AI.GPT.Interface.ILogger;
using ILoggerFactory = Devon.Azure.AI.GPT.Interface.ILoggerFactory;
namespace Devon.Azure.AI.GPT.Factory
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
