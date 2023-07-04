using Devon.Azure.AI.GPT.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devon.Azure.AI.GPT.Services.Interface
{
    public interface IHomeService
    {
        JsonResult GetEOLGPTResponse(List<ChatHistory> conversations, string userInput, string systemMessage = "");
    }
}