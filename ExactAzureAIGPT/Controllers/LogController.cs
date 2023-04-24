using ExactAzureAIGPT.Filter;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using Microsoft.AspNetCore.Http;

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
                string dateFormat = "yyyy-MM-dd";
                string logDirectory = _configuration.GetValue<string>("LogFilePath");
                if (Directory.Exists(logDirectory))
                {

                    if (string.IsNullOrEmpty(date))
                    {
                        date = $"{DateTime.Now:yyyy-MM-dd}";
                    }

                    bool isValidDate = IsValidDateFormat(date, dateFormat);

                    if (isValidDate)
                    {
                        string logFilePath = Path.Combine(logDirectory, $"{date}.log");
                        string fileContent;
                        using (StreamReader reader = new StreamReader(logFilePath))
                        {
                            fileContent = reader.ReadToEnd();
                            return Content(fileContent, "text/plain");
                        }
                    }
                    else
                    {
                        return Content("Please provide date in yyyy-MM-dd format");
                    }
                }
                else
                {
                    Directory.CreateDirectory(logDirectory);
                    if (string.IsNullOrEmpty(date))
                    {
                        date = $"{DateTime.Now:yyyy-MM-dd}";
                    }

                    bool isValidDate = IsValidDateFormat(date, dateFormat);

                    if (isValidDate)
                    {
                        string logFilePath = Path.Combine(logDirectory, $"{date}.log");
                        string fileContent;
                        using (StreamReader reader = new StreamReader(logFilePath))
                        {
                            fileContent = reader.ReadToEnd();
                            return Content(fileContent, "text/plain");
                        }
                    }
                    else
                    {
                        return Content("Please provide date in yyyy-MM-dd format");
                    }
                }

            }
            catch (FileNotFoundException)
            {
                return Content("File Not found: " + date);
            }
            catch (Exception)
            {
                return Content("Please provide date in yyyy-MM-dd format");
            }

        }

        static bool IsValidDateFormat(string dateString, string dateFormat)
        {
            DateTime result;
            return DateTime.TryParseExact(dateString, dateFormat, null, System.Globalization.DateTimeStyles.None, out result);
        }
    }
}
