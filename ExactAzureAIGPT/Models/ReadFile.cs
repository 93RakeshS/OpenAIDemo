namespace ExactAzureAIGPT.Models
{
    public class ReadFile
    {
        public string ReadContentofFile(string fileName)
        {
          return  File.ReadAllText("C:\\JsonFile\\"+fileName);
        }

        public void WriteContentsToFile(string content) 
        {
            string chatHisoryFolder = "chatHistory";
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string chatHistory = $"chatHistory_{today}.txt";
            string filePath = Path.Combine(chatHisoryFolder, chatHistory);
            File.AppendAllText(filePath, content);
        }
    }
}
