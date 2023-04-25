using Azure;
using Azure.AI.OpenAI;
using ExactAzureAIGPT.Helpers;
using ExactAzureAIGPT.Models;
using ExactAzureAIGPT.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ExactAzureAIGPT.Services.Class
{
    public class HomeService : IHomeService
    {
        private readonly IConfiguration _configuration;
        public HomeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JsonResult GetEOLGPTResponse(List<ChatHistory> conversations, string userInput, string systemMessage)
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

                return new JsonResult(content);
        }
    }
}