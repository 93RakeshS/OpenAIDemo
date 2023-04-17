namespace ExactOpenAIDemo.Model
{
    public class ReadFile
    {
        public string ReadContentOfTheFile()
        {
            string filePath = "C:\\JsonFile\\Person.json";
            string jsonText = File.ReadAllText(filePath);
            return jsonText;
        }
    }
}
