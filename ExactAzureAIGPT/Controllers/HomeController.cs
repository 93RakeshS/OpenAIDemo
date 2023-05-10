using ExactAzureAIGPT.Filter;
using ExactAzureAIGPT.Models;
using ExactAzureAIGPT.Services.Interface;
using Microsoft.AspNetCore.Mvc;
namespace ExactAzureAIGPT.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService, IConfiguration configuration)
        {
            _homeService = homeService;
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
        public JsonResult GetEOLGPTResponse(List<ChatHistory> conversations, string userInput, string systemMessage = "")
        {
            try
            {
                return _homeService.GetEOLGPTResponse(conversations, userInput, systemMessage);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }
    }
}