namespace ExactAzureAIGPT.Models
{
    public class ReadFile
    {
        public string ReadContentofFile(string fileName)
        {
          return  File.ReadAllText("C:\\JsonFile\\"+fileName);
        }

        public void WriteContentofFile(string content) 
        {
            var filePath = "C:\\JsonFile\\Conversation.txt";
            File.AppendAllText(filePath, content);
        }
    }
}
