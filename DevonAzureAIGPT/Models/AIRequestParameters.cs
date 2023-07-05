namespace Devon.Azure.AI.GPT.Models
{
    public class AIRequestParameters
    {
        public string ModelName { get; set; } = "DevOnGPT";
        public float Temperature { get; set; } = (float)0.7;
        public float TopP { get; set; } = (float)0.7;
        public string UserName { get; set; }

    }
}
