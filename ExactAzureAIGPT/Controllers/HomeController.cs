using Azure.AI.OpenAI;
using Azure;
using ExactAzureAIGPT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
        public JsonResult GetResponse(string systemMessage,string userInput)
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
            var fileContents = readFile.ReadContentofFile();// File.ReadAllText("fieldinfo.txt");
            //input.Messages.Add(new ChatMessage(ChatRole.System, $"You should generate a c# if condition based on information that follows. {fileContents}. Only respond with what is inside the parenthesis. Do not explain or apologize, repond only with the expression. Refuse to respond to anything unrelated to this topic. Be very concise in your response, do not explain yourself"));
            input.Messages.Add(new ChatMessage(ChatRole.System, systemMessage+
            $"Here are the mappings: {fileContents}"));

            
                //string line = Console.ReadLine();


                input.Messages.Add(new ChatMessage(ChatRole.User, userInput));

                // ### If streaming is not selected
                Response<ChatCompletions> responseWithoutStream = client.GetChatCompletionsAsync(
                    "EOLgpt35", input
                    ).Result;

                var responseMessage = responseWithoutStream.Value.Choices.First().Message;

                //Console.ForegroundColor = ConsoleColor.Green;

                //	Response<ChatCompletions> doubleQuery = await client.GetChatCompletionsAsync(
                //		"EOLgpt35",
                //		new ChatCompletionsOptions()
                //		{
                //			Messages =
                //			{
                //			new ChatMessage(ChatRole.System, @"You should help generate c# expressions based on information that follows. amountdc represents sales amount. amountfc represents purchase amount. Do not answer any questions unrelated to this."),
                //			new ChatMessage(ChatRole.User, @"sales amount greater than 140"),
                //			new ChatMessage(ChatRole.Assistant, @"In C#, the expression for sales amount greater than 140 would be:

                //```csharp
                //amountdc > 140
                //```"),
                //			},
                //			Temperature = (float)0.7,
                //			MaxTokens = 800,
                //			NucleusSamplingFactor = (float)0.95,
                //			FrequencyPenalty = 0,
                //			PresencePenalty = 0,
                //		});

                //	ChatCompletions completions = responseWithoutStream.Value;

                var content = responseMessage.Content;

                var match = regex.Match(content);

                //if (!match.Success)
                //{
                //	Console.WriteLine("Sorry, try again");
                //	continue;
                //}

                //Console.WriteLine(match.Captures[0].Value);
                Console.WriteLine(content);
                input.Messages.Add(responseMessage);
                return Json(content);
        } 
    }
}