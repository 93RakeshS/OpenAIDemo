using Exact.Azure.AI.GPT.Models;
namespace Exact.Azure.AI.GPT.Services.Interface
{
    public interface IAIService
    {
        Task<string> GetResponseFromAI(GPTConversation conversations, AIRequestParameters aiRequestParameters);
    }
}