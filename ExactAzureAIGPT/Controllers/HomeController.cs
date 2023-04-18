using Azure.AI.OpenAI;
using Azure;
using ExactAzureAIGPT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExactAzureAIGPT.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
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
        [HttpPost]
        public JsonResult GetResponse( string userInput = "", string systemMessage = "", string history="", List<string>? shotMessagesUser = null, List<string>? shotMessagesAssistant = null)
        {
            try
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

                
                if(shotMessagesUser != null && shotMessagesAssistant != null)
                {
                    for(var i = 0 ; i < shotMessagesUser.Count;i++) 
                    {
                        input.Messages.Add(new ChatMessage(ChatRole.User, shotMessagesUser[i]));
                        input.Messages.Add(new ChatMessage(ChatRole.Assistant, shotMessagesAssistant[i]));
                    }
                    //foreach(var shotMessageUser in shotMessagesUser) 
                    //{
                    //    input.Messages.Add(new ChatMessage(ChatRole.User, shotMessageUser));
                    //}
                    //foreach(var shotMessageAssistant in shotMessagesAssistant)
                    //{
                    //    input.Messages.Add(new ChatMessage(ChatRole.Assistant, shotMessageAssistant));
                    //}
                }

                input.Messages.Add(new ChatMessage(ChatRole.System, systemMessage));
                
                if (history != "")
                {
                    input.Messages.Add(new ChatMessage(ChatRole.System, systemMessage));
                    var historyList = BuidlChatHistory(history);
                    foreach (var historyItem in historyList)
                    {
                        input.Messages.Add(new ChatMessage(ChatRole.Assistant, historyItem.Assistant));
                        input.Messages.Add(new ChatMessage(ChatRole.User, historyItem.User));
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
            catch(Exception ex)
            {
                return Json(ex.Message);
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