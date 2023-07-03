using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Exact.Azure.AI.GPT.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;
        protected readonly IMemoryCache _cache;
		public BaseController(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration;
			_cache = cache;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			string email = LoggedInUser();
			if (_configuration["AllowedAdminUser"].Contains(email))
			{
				ViewBag.LoggedInUser = email;
				ViewBag.IsUserAllowed = true;
				base.OnActionExecuting(context);
			}
			else
			{
                ViewBag.LoggedInUser = email;
                ViewBag.IsUserAllowed = false;
                base.OnActionExecuting(context);
            }			
		}

		public string LoggedInUser()
		{
			IConfigurationSection machineUserMappingSection = _configuration.GetSection("MachineUserMapping");
			var machineUserMapping = machineUserMappingSection.Get<Dictionary<string, string>>();

			string machineName = Environment.MachineName;
			if (machineUserMapping.ContainsKey(machineName))
			{
				return machineUserMapping[machineName];
			}

			var key = Request.Cookies["AppServiceAuthSession"];
			string userName;
			if (_cache.TryGetValue(key, out userName))
			{

			}
			else
			{
				HttpClient client = new()
				{
					BaseAddress = new Uri("https://eolpocgpt.azurewebsites.net/")
				};

				client.DefaultRequestHeaders.Add("Cookie", Request.Headers["Cookie"].ToString());

				var res = client.GetAsync(".auth/me").Result;

				var json = res.Content.ReadAsStringAsync().Result;

				var obj = JsonSerializer.Deserialize<AdUser[]>(json);
				userName = obj[0].user_id;
				_cache.Set(key, obj[0].user_id, new DateTimeOffset(DateTime.Now.AddMinutes(120)));
			}

			return userName;
		}

		public class AdUser
		{
			public string user_id { get; set; }
		}

	}
}