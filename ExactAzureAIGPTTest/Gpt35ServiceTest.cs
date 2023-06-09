﻿using Exact.Azure.AI.GPT.Models;
using Exact.Azure.AI.GPT.Services.Class;
using Exact.Azure.AI.GPT.Tests;
using Microsoft.Extensions.Configuration;
using Moq;
using ILogger = Exact.Azure.AI.GPT.Interface.ILogger;

namespace ExactAzureAIGPTTest
{
    public class Gpt35ServiceTest
    {
        private readonly Gpt35Service _service;
        private readonly Mock<ILogger> _logger;

        public Gpt35ServiceTest()
        {
            var myConfiguration = new Dictionary<string, string>
                {
                    {"AzureOpenAIKey", Utility.apikey},
                    {"AzureOpenAIurl", Utility.apiurl}
                };
            _logger = new Mock<ILogger>();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
            _service = new Gpt35Service(configuration, _logger.Object);
        }
        [Fact]
        public async void GetEOLGPTResponse_ReturnsExpectedResults()
        {
            List<ChatHistory> chatHistories = new List<ChatHistory>();
            string inputPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Gpt35Query");
            string systemMessagePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "SystemMessage", "Gpt35SystemMessage.txt");
            string[] files = Directory.GetFiles(inputPath);
            var aiRequestParameters = new AIRequestParameters
            {
                ModelName = "EOLgpt35",
                Temperature = (float)0.0,
                TopP = (float)0.5,
            };
            foreach (string file in files)
            {
                inputPath = Path.Combine(inputPath, file);
                foreach (RequestResponse item in Utility.GetData(inputPath))
                {
                    var conversation = new GPTConversation
                    {
                        ChatHistories = chatHistories,
                        UserInput = item.UserRequest,
                        SystemMessage = Utility.ReadSystemMessage(systemMessagePath)
                    };
                    var response = await _service.GetResponseFromAI(conversation, aiRequestParameters);
                    Assert.Contains(item.UserResponse, response.Replace("\n", ""));
                    //Assert.Equal(item.UserResponse, response.Replace("\n", ""));
                    chatHistories.Add(new ChatHistory { User = item.UserRequest, Assistant = item.UserResponse });
                }
            }
        }
    }
}