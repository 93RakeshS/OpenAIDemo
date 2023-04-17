using Azure.AI.OpenAI;
using Azure;
using ExactAzureAIGPT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;

namespace ExactAzureAIGPT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public JsonResult GetResponse(string systemMessage, string userInput)
        {
            OpenAIClient client = new OpenAIClient(
            new Uri("https://eolai.openai.azure.com/"),
            new AzureKeyCredential("4e248eeb1cd9440e8933201836a72bbd"));

            var input = new ChatCompletionsOptions()
            {
                Temperature = (float)0.7,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,

            };

            var regex = new Regex(@"if.+\((.+)\).+\{.+\}");
            ReadFile readFile = new ReadFile();
            var readFileContents = readFile.ReadContentofFile("fieldInfo.txt");
            var readFileContentConvo = readFile.ReadContentofFile("Conversation.txt");// File.ReadAllText("fieldinfo.txt");
            readFile.WriteContentofFile("\nUser :"+"\n"+userInput);
            
            input.Messages.Add(new ChatMessage(ChatRole.System, systemMessage +
            $"Here are the mappings: {readFileContents}"));
            
            input.Messages.Add(new ChatMessage(ChatRole.User, $"Here is the latest conversation : {readFileContentConvo}"));

            input.Messages.Add(new ChatMessage(ChatRole.User, userInput));

            // ### If streaming is not selected
            Response<ChatCompletions> responseWithoutStream = client.GetChatCompletionsAsync(
                "EOLgpt35", input
                ).Result;

            var responseMessage = responseWithoutStream.Value.Choices.First().Message;

            var content = responseMessage.Content;

            var match = regex.Match(content);

            //Console.WriteLine(match.Captures[0].Value);
            readFile.WriteContentofFile("\nGPT :"+"\n"+content);
            input.Messages.Add(responseMessage);
            return Json(content);
        }
    }
}