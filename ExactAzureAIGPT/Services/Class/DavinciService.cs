using Azure;
using Azure.AI.OpenAI;
using Exact.Azure.AI.GPT.Helpers;
using Exact.Azure.AI.GPT.Models;
using Exact.Azure.AI.GPT.Services.Interface;
using ILogger = Exact.Azure.AI.GPT.Interface.ILogger;

namespace Exact.Azure.AI.GPT.Services.Class
{
    public class DavinciService : IAIService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public DavinciService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<string> GetResponseFromAI(GPTConversation conversations, AIRequestParameters aiRequestParameters)
        {
            string prompt = conversations.SystemMessage;
            if (conversations.ChatHistories != null)
            {
                foreach (var conversation in conversations.ChatHistories)
                {
                    prompt = prompt + "\nQ:" + conversation.User;
                    prompt = prompt + "\nA:" + conversation.Assistant.SanitizeHtml();
                }
            }
            prompt = prompt + "\nQ:" + conversations.UserInput;

            OpenAIClient client = new OpenAIClient(new Uri(_configuration["AzureOpenAIurl"]),
				new AzureKeyCredential(_configuration["AzureOpenAIKey"]));
				
				var input = new CompletionsOptions()
				{
                Prompts = { prompt },
					Temperature = aiRequestParameters.Temperature,
					MaxTokens = 100,
					NucleusSamplingFactor = aiRequestParameters.TopP,
					FrequencyPenalty = (float)0,
					PresencePenalty = (float)0,
                GenerationSampleCount = 1,
				};

				Response<Completions> completionsResponse = await client.GetCompletionsAsync(
									aiRequestParameters.ModelName, input
									);

            Completions completions = completionsResponse.Value;
            var response = completionsResponse.Value.Choices[0].Text;
				_logger.LogInfo("-------------------Davinci-------------------");
				_logger.LogInfo("System : " + conversations.SystemMessage);
				_logger.LogInfo($"{aiRequestParameters.UserName} : {conversations.UserInput}");
				_logger.LogInfo("Assistant : " + response);
				return response.Replace("A:", "");
			}
    }
}
