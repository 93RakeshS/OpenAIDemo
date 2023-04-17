namespace ExactAzureAIGPT.Models
{
    public class ReadFile
    {
        public string ReadContentofFile()
        {
          return  File.ReadAllText("C:\\JsonFile\\fieldinfo.txt");
        }
    }
}
