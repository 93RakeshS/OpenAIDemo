using ExactAzureAIGPT.Factory;
using ILogger = ExactAzureAIGPT.Interface.ILogger;
using ILoggerFactory = ExactAzureAIGPT.Interface.ILoggerFactory;

namespace ExactAzureAIGPT.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private string providerType;

        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            this.providerType = "text";
            _logger = loggerFactory.CreateLogger("text");
            
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
        }
    }
}
