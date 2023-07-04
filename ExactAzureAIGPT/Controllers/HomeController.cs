using Exact.Azure.AI.GPT.Factory;
using Exact.Azure.AI.GPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;



namespace Exact.Azure.AI.GPT.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ServiceFactory _serviceFactory;
        public HomeController(ServiceFactory serviceFactory, IConfiguration configuration, IMemoryCache cache)
            : base(configuration, cache)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetResponseFromAI(GPTConversation conversations, AIRequestParameters aiRequestParameters)
        {
            try
            {
                aiRequestParameters.UserName = base.ViewBag.LoggedInUser;
                var result = _serviceFactory.GetService(aiRequestParameters.ModelName).GetResponseFromAI(conversations, aiRequestParameters).Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}