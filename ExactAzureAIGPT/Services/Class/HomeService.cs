using Azure;
using Azure.AI.OpenAI;
using ExactAzureAIGPT.Helpers;
using ExactAzureAIGPT.Models;
using ExactAzureAIGPT.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using ILogger = ExactAzureAIGPT.Interface.ILogger;

namespace ExactAzureAIGPT.Services.Class
{
    public class HomeService : IHomeService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public HomeService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        /// <summary>
        /// GetEOLGPTResponse
        /// </summary>
        /// <param name="conversations">contains shot messages and history of conversation between gpt and user</param>
        /// <param name="userInput">question/request by user</param>
        /// <param name="systemMessage">System Message to provide context to gpt</param>
        /// <returns>Json Result that contains reponse to the specific user input</returns>
        public JsonResult GetEOLGPTResponse(List<ChatHistory> conversations, string userInput, string systemMessage)
        {
            try
            {
                OpenAIClient client = new OpenAIClient(
                new Uri(_configuration["AzureOpenAIurl"]),
                new AzureKeyCredential(_configuration["AzureOpenAIKey"]));

                var input = new ChatCompletionsOptions()
                {
                    Temperature = (float)0.7,
                    MaxTokens = 800,
                    NucleusSamplingFactor = (float)0.95,
                    FrequencyPenalty = 0,
                    PresencePenalty = 0,
                };

                if (!string.IsNullOrEmpty(systemMessage))
                    input.Messages.Add(new ChatMessage(ChatRole.System, systemMessage));

                if (conversations.Any())
                {
                    foreach (var conversation in conversations)
                    {
                        input.Messages.Add(new ChatMessage(ChatRole.User, conversation.User));
                        input.Messages.Add(new ChatMessage(ChatRole.Assistant, conversation.Assistant.SanitizeHtml()));
                    }
                }

                input.Messages.Add(new ChatMessage(ChatRole.User, userInput));

                Response<ChatCompletions> response = client.GetChatCompletionsAsync(
                    "EOLgpt35", input
                    ).Result;

                var responseMessage = response.Value.Choices.First().Message;

                var content = responseMessage.Content.ReplaceNewLineWithHtmlBreak();
                _logger.LogInfo("System : " + systemMessage);
                _logger.LogInfo("User : " + userInput);
                _logger.LogInfo("Assistant : " + content);

                return new JsonResult(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new JsonResult(ex.Message);
            }
        }
    }
}