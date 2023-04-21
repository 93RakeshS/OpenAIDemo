using ExactAzureAIGPT.Filter;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;

namespace ExactAzureAIGPT.Controllers
{
    public class LogController : Controller
    {
        private readonly IConfiguration _configuration;

        public LogController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AuthorizedFilter]
        [HttpGet]
        public IActionResult Index(string date = "")
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                {
                    date = $"{DateTime.Now:yyyy-MM-dd}";
                }
                string logDirectory = _configuration.GetValue<string>("LogFilePath");
                string logFilePath = Path.Combine(logDirectory, $"{date}.log");
                string fileContent;
                using (StreamReader reader = new StreamReader(logFilePath))
                {
                    fileContent = reader.ReadToEnd();
                    return Content(fileContent, "text/plain");
                }

            }
            catch (FileNotFoundException)
            {
                return Content("File Not found: " + date);
            }
            catch (Exception ex)
            {
                return Content("Please provide date in yyyy-MM-dd format");
            }

        }
    }
}
