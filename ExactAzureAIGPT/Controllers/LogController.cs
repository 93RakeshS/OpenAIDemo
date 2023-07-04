using Devon.Azure.AI.GPT.Filter;
using Microsoft.AspNetCore.Mvc;
using Devon.Azure.AI.GPT.Constants;
using Microsoft.Extensions.Caching.Memory;

namespace Devon.Azure.AI.GPT.Controllers
{
    public class LogController : BaseController
    {
        public LogController(IConfiguration configuration, IMemoryCache cache):base(configuration,cache)
        {
            //_configuration = configuration;
        }

        [AuthorizedFilter]
        [HttpGet]
        public IActionResult Index(string date = "")
        {
            try
            {
                string dateFormat = "yyyy-MM-dd";
                string logDirectory = _configuration.GetValue<string>("LogFilePath");

                if (string.IsNullOrEmpty(date))
                {
                    date = $"{DateTime.Now:yyyy-MM-dd}";
                }

                bool isValidDate = IsValidDateFormat(date, Constant.DateFormat);

                if (isValidDate)
                {
                    string logFilePath = Path.Combine(logDirectory, $"eolgpt_{date}.log");
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
