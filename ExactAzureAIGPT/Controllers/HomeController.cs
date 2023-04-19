using Azure;
using Azure.AI.OpenAI;
using ExactAzureAIGPT.Filter;
using ExactAzureAIGPT.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExactAzureAIGPT.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
        [AuthorizedFilter]
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }


        [HttpPost]
        public JsonResult GetResponse(List<ChatHistory> conversations, string userInput, string systemMessage = "")
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

                if(!string.IsNullOrEmpty(systemMessage)) 
                    input.Messages.Add(new ChatMessage(ChatRole.System, systemMessage));
                
                if (conversations.Any())
                {
                    foreach (var conversation in conversations)
                    {
                        input.Messages.Add(new ChatMessage(ChatRole.User, conversation.User));
                        input.Messages.Add(new ChatMessage(ChatRole.Assistant, conversation.Assistant));
                    }
                }

                input.Messages.Add(new ChatMessage(ChatRole.User, userInput));

                Response<ChatCompletions> response = client.GetChatCompletionsAsync(
                    "EOLgpt35", input
                    ).Result;

                var responseMessage = response.Value.Choices.First().Message;

                var content = responseMessage.Content;

                return Json(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }
        }
    }
}