using Exact.Azure.AI.GPT.Services.Class;
using Exact.Azure.AI.GPT.Services.Interface;
using LoggerFactory = Exact.Azure.AI.GPT.Factory.LoggerFactory;
using ILoggerFactory = Exact.Azure.AI.GPT.Interface.ILoggerFactory;
using Exact.Azure.AI.GPT.Factory;

namespace Exact.Azure.AI.GPT.Extensions
{ 
    public static class RegisterDependenciesExtension
    {
        public static void AddDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
            builder.Services.AddSingleton(provider => provider.GetRequiredService<ILoggerFactory>()
             .CreateLogger(builder.Configuration.GetValue<string>("LoggerType")));

            builder.Services.AddScoped<ServiceFactory>();
            builder.Services.AddScoped<Gpt35Service>();
            builder.Services.AddScoped<DavinciService>();

        }
    }
}