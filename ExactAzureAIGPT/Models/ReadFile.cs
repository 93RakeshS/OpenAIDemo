using System.IO;

namespace Exact.Azure.AI.GPT.Models
{
    public class ReadFile
    {
        public string ReadContentofFile(string fileName)
        {
          return  File.ReadAllText("C:\\JsonFile\\"+fileName);
        }

        public void WriteContentsToFile(string content) 
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var newPath = baseDirectory.Replace(Path.GetRelativePath("Exact.Azure.AI.GPT", baseDirectory).Split("..")[1], "");
            string chatHistoryFolder = Path.Combine(newPath, "chatHistory");
            if (!Directory.Exists(chatHistoryFolder))
            {
                Directory.CreateDirectory(chatHistoryFolder);
            }

            //string chatHisoryFolder = "chatHistory";
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string chatHistory = $"chatHistory_{today}.txt";
            string filePath = Path.Combine(chatHistoryFolder, chatHistory);
            File.AppendAllText(filePath, content);
        }
    }
}
