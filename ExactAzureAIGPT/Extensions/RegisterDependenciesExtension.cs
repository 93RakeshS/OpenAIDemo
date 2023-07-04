using Devon.Azure.AI.GPT.Services.Class;
using Devon.Azure.AI.GPT.Services.Interface;
using LoggerFactory = Devon.Azure.AI.GPT.Factory.LoggerFactory;
using ILoggerFactory = Devon.Azure.AI.GPT.Interface.ILoggerFactory;
using Devon.Azure.AI.GPT.Factory;

namespace Devon.Azure.AI.GPT.Extensions
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