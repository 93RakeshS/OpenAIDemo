using ExactAzureAIGPT.Services.Class;
using ExactAzureAIGPT.Services.Interface;
using LoggerFactory = ExactAzureAIGPT.Factory.LoggerFactory;
using ILoggerFactory = ExactAzureAIGPT.Interface.ILoggerFactory;
namespace ExactAzureAIGPT.Extensions
{
    public static class RegisterDependenciesExtension
    {
        public static void AddDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
            builder.Services.AddSingleton(provider => provider.GetRequiredService<ILoggerFactory>()
             .CreateLogger(builder.Configuration.GetValue<string>("LoggerType")));
            builder.Services.AddScoped<IHomeService, HomeService>();
        }
    }
}