using Exact.Azure.AI.GPT.Models;
using Exact.Azure.AI.GPT.Services.Class;
using Microsoft.Extensions.Configuration;
using Moq;
using ILogger = Exact.Azure.AI.GPT.Interface.ILogger;

namespace Exact.Azure.AI.GPT.Tests
{
	public class DavinciServiceTest
	{
		private readonly DavinciService _service;
		private readonly Mock<ILogger> _logger;

		public DavinciServiceTest()
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
			//Arrange
			_service = new DavinciService(configuration, _logger.Object);
		}
		[Fact]
		public async void Get_DavinciResponse_ExpectedResult()
		{
			List<ChatHistory> chatHistories = new List<ChatHistory>();
			string inputPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "DavinciQuery");
			string systemMessagePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "SystemMessage", "DavinciSystemMessage.txt");
			string[] files = Directory.GetFiles(inputPath);

			var aiRequestParameters = new AIRequestParameters
			{
				ModelName = "text-davinci-003",
				Temperature = (float)0.0,
				TopP = (float)0.5,
				UserName = "demoUser"
			};


			foreach (string file in files)
			{
				chatHistories = new List<ChatHistory>();
				inputPath = Path.Combine(inputPath, file);
				foreach (RequestResponse item in Utility.GetData(inputPath))
				{
					var conversation = new GPTConversation
					{
						ChatHistories = chatHistories,
						UserInput = item.UserRequest,
						SystemMessage = Utility.ReadSystemMessage(systemMessagePath)
					};
					//Act
					var result = await _service.GetResponseFromAI(conversation, aiRequestParameters);

					//string response = Convert.ToString(result.Value);
					//Assert
					Assert.Equal(item.UserResponse, result.Replace(".\n", "").Replace("\n", ""));
					chatHistories.Add(new ChatHistory { User = item.UserRequest, Assistant = item.UserResponse });
				}
			}
		}
	}
}