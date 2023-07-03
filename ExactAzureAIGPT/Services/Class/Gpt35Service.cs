using Azure;
using Azure.AI.OpenAI;
using Exact.Azure.AI.GPT.Helpers;
using Exact.Azure.AI.GPT.Models;
using Exact.Azure.AI.GPT.Services.Interface;
using ILogger = Exact.Azure.AI.GPT.Interface.ILogger;

namespace Exact.Azure.AI.GPT.Services.Class
{
    public class Gpt35Service : IAIService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public Gpt35Service(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        /// <summary>
        /// GetResponseFromAI
        /// </summary>
        /// <param name="conversations">contains shot messages and history of conversation between gpt and user</param>
        /// <param name="aiRequestParameters">question/request by user</param>
        /// <returns>string that contains reponse to the specific user input</returns>
        public async Task<string> GetResponseFromAI(GPTConversation conversations, AIRequestParameters aiRequestParameters)
        {
            try
            {
                OpenAIClient client = new OpenAIClient(
                new Uri(_configuration["AzureOpenAIurl"]),
                new AzureKeyCredential(_configuration["AzureOpenAIKey"]));

                var input = new ChatCompletionsOptions()
                {
                    Temperature = aiRequestParameters.Temperature,
                    MaxTokens = 800,
                    NucleusSamplingFactor = aiRequestParameters.TopP,
                    FrequencyPenalty = 0,
                    PresencePenalty = 0,
                };

                if (!string.IsNullOrEmpty(conversations.SystemMessage))
                    input.Messages.Add(new ChatMessage(ChatRole.System, conversations.SystemMessage));

                if (conversations.ChatHistories != null)
                {
                    foreach (var conversation in conversations.ChatHistories)
                    {
                        input.Messages.Add(new ChatMessage(ChatRole.User, conversation.User));
                        input.Messages.Add(new ChatMessage(ChatRole.Assistant, conversation.Assistant.SanitizeHtml()));
                    }
                }

                input.Messages.Add(new ChatMessage(ChatRole.User, conversations.UserInput));

                Response<ChatCompletions> response = await client.GetChatCompletionsAsync(
                    aiRequestParameters.ModelName, input
                    );

                var responseMessage = response.Value.Choices.First().Message;
				
				var content = responseMessage.Content.ReplaceNewLineWithHtmlBreak();
                _logger.LogInfo("-------------------GPT35-------------------");
                _logger.LogInfo("System : " + conversations.SystemMessage);
				_logger.LogInfo($"{aiRequestParameters.UserName} : {conversations.UserInput}");
				_logger.LogInfo("Assistant : " + content);

                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ex.Message;
            }
        }

        public string GetName()
        {
            return "Gpt35Service";
        }
    }
}