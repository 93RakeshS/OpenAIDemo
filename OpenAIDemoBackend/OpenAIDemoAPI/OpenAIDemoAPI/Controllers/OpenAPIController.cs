using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;

namespace OpenAIDemoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenAPIController : ControllerBase
    {
        private readonly ILogger<OpenAPIController> _logger;
        private readonly IConfiguration _configuration;
        public OpenAPIController(ILogger<OpenAPIController> logger, IConfiguration configuration) 
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> ReturnAResponseBasedOnInput(string input) 
        {
            var gpt3 = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = _configuration["GPTSecretKey"],
            });
            var completionResult = await gpt3.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = input,
                Model = Models.ChatGpt3_5Turbo,
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
            return Ok(input);
        }
    }
}
