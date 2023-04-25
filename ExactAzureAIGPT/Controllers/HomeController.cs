using Azure;
using Azure.AI.OpenAI;
using ExactAzureAIGPT.Filter;
using ExactAzureAIGPT.Helpers;
using ExactAzureAIGPT.Models;
using ExactAzureAIGPT.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ExactAzureAIGPT.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService, IConfiguration configuration)
        {
            _homeService = homeService;
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
                return _homeService.GetEOLGPTResponse(conversations, userInput, systemMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }
        }
    }
}