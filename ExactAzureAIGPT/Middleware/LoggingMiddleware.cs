using Devon.Azure.AI.GPT.Factory;
using ILogger = Devon.Azure.AI.GPT.Interface.ILogger;
using ILoggerFactory = Devon.Azure.AI.GPT.Interface.ILoggerFactory;

namespace Devon.Azure.AI.GPT.Middleware
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
