using Azure.AI.OpenAI;
using Azure;
using ExactAzureAIGPT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using ExactAzureAIGPT.Filter;

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
        public JsonResult GetResponse( string userInput = "", string systemMessage = "", string history="", List<string>? shotMessagesUser = null, List<string>? shotMessagesAssistant = null)
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

                input.Messages.Add(new ChatMessage(ChatRole.System, systemMessage));

                if (shotMessagesUser != null && shotMessagesAssistant != null)
                {
                    for(var i = 0 ; i < shotMessagesUser.Count;i++) 
                    {
                        input.Messages.Add(new ChatMessage(ChatRole.User, shotMessagesUser[i]));
                        input.Messages.Add(new ChatMessage(ChatRole.Assistant, shotMessagesAssistant[i]));
                    }
                }

                if (history != "")
                {
                    var historyList = BuidlChatHistory(history);
                    foreach (var historyItem in historyList)
                    {   
                        input.Messages.Add(new ChatMessage(ChatRole.User, historyItem.User));
                        input.Messages.Add(new ChatMessage(ChatRole.Assistant, historyItem.Assistant));
                    }
                }
                input.Messages.Add(new ChatMessage(ChatRole.User, userInput));

                Response<ChatCompletions> responseWithoutStream = client.GetChatCompletionsAsync(
                    "EOLgpt35", input
                    ).Result;

                var responseMessage = responseWithoutStream.Value.Choices.First().Message;

                var content = responseMessage.Content;

                input.Messages.Add(responseMessage);
                return Json(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json("try again");
            }
        }

        private List<ChatHistory> BuidlChatHistory(string chatHistoryText)
        {
            List<ChatHistory> chatHistory = new List<ChatHistory>();
            string[] chatHistoryLines = chatHistoryText.Trim().Split('\n');

            for (int i = 0; i < chatHistoryLines.Length; i += 2)
            {
                string userMessage = chatHistoryLines[i].Replace("U:", "").Trim();
                string assistantMessage = chatHistoryLines[i + 1].Replace("A:", "").Trim();

                chatHistory.Add(new ChatHistory { User = userMessage, Assistant = assistantMessage });
            }

            return chatHistory;
        }

    }  
 }