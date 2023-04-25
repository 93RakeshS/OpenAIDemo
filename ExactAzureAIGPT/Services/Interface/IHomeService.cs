using ExactAzureAIGPT.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExactAzureAIGPT.Services.Interface
{
    public interface IHomeService
    {
        JsonResult GetEOLGPTResponse(List<ChatHistory> conversations, string userInput, string systemMessage = "");
    }
}