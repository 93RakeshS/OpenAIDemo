using Exact.Azure.AI.GPT.Factory;
using ILogger = Exact.Azure.AI.GPT.Interface.ILogger;
using ILoggerFactory = Exact.Azure.AI.GPT.Interface.ILoggerFactory;

namespace Exact.Azure.AI.GPT.Middleware
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
