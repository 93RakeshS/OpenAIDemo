using Devon.Azure.AI.GPT.Models;
namespace Devon.Azure.AI.GPT.Services.Interface
{
    public interface IAIService
    {
        Task<string> GetResponseFromAI(GPTConversation conversations, AIRequestParameters aiRequestParameters);
    }
}