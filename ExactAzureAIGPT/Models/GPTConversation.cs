namespace Exact.Azure.AI.GPT.Models
{
    public class GPTConversation
    {
        public string UserInput { get; set; }
        public string SystemMessage{ get; set; }
        public List<ChatHistory> ChatHistories{ get; set; }
    }
}