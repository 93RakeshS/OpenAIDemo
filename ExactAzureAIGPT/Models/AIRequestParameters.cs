namespace Exact.Azure.AI.GPT.Models
{
    public class AIRequestParameters
    {
        public string ModelName { get; set; } = "EOLgpt35";
        public float Temperature { get; set; } = (float)0.7;
        public float TopP { get; set; } = (float)0.7;
    }
}
