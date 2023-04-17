using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using ExactOpenAIDemo.Model;
using System.Diagnostics;
using Newtonsoft.Json;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace ExactOpenAIDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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
        public async Task<JsonResult> ReadfromExternalInput(string input)
        {
            string externalData = "menu: Pizza Margherita, Pizza Pepperoni, Garlic Bread, Caesar Salad\nuser name: John Doe\naddress: 123 Main St.";

            var gpt3 = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = _configuration["GPTSecretKey"],
            });
            var prompt = $"{input} {externalData}";

            var completionResult = await gpt3.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = input,
                Model = Models.TextDavinciV2,
                Temperature = 0.5F,
                MaxTokens = 100
            });
            var result = new List<string>();
            if (completionResult.Successful)
            {
                foreach (var choice in completionResult.Choices)
                {
                    Console.WriteLine(choice.Text);
                    result.Add(choice.Text);
                }
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }
                result.Add($"Error {completionResult.Error}");
                Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
            }
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> ReturnResponse(string input)
        {
            string query="";
            var result = new List<string>();
            // Set up OpenAI API credentials
            var gpt3 = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = _configuration["GPTSecretKey"],
            });

            // Load the external JSON file
            ReadFile readFile=new ReadFile();
            string jsonText = readFile.ReadContentOfTheFile();

            // Deserialize the JSON data into a C# object
            try
            {
                //dynamic jsonData = JsonConvert.DeserializeObject(jsonText);
                query = Convert.ToString(jsonText)+"\n"+input;
                //if (input.Contains("Age".Trim()) || input.Contains("age".Trim()))
                //{
                //    query = $"What is {jsonData.Age}?";
                //}
                //if (input.Contains("name".Trim()) || input.Contains("Name".Trim()))
                //{

                //    query = $"What is {jsonData.Name}?";
                //}
                //if (input.Contains("Occupation".Trim()) || input.Contains("occupation".Trim()))
                //{
                //    query = $"What is {jsonData.Occupation}?";
                //}
                //if (input.Contains("Skill".Trim()) || input.Contains("skill".Trim()))
                //{
                //    query = $"What is {jsonData.Skill}?";
                //}
            }
           
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message );
            }
            // Generate a response for the query using OpenAI's GPT model
            var inputOptions = new CompletionCreateRequest()
            {
                Prompt = query,
                Model = Models.TextDavinciV3,
                Temperature = 0.5F,
                MaxTokens = 1024,
            };
  
            var completionResult = await gpt3.Completions.CreateCompletion(inputOptions);
                        
            if (completionResult.Successful)
            {
                foreach (var choice in completionResult.Choices)
                {
                    Console.WriteLine(choice.Text);
                    readFile.WriteContentOfTheFile(choice.Text);
                    result.Add(choice.Text);
                }
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }
                result.Add($"Error {completionResult.Error}");
                Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
            }
            return Json(result);
        }
    }
}