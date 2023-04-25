using ExactAzureAIGPT.Services.Class;
using ExactAzureAIGPT.Services.Interface;

namespace ExactAzureAIGPT.Extensions
{
    public static class RegisterDependenciesExtension
    {
        public static void AddDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IHomeService, HomeService>();
        }
    }
}