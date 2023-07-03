using Exact.Azure.AI.GPT.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exact.Azure.AI.GPT.Services.Interface
{
    public interface IHomeService
    {
        JsonResult GetEOLGPTResponse(List<ChatHistory> conversations, string userInput, string systemMessage = "");
    }
}