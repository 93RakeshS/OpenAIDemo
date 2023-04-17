namespace ExactOpenAIDemo.Model
{
    public class ReadFile
    {
        public string ReadContentOfTheFile()
        {
            string filePath = "C:\\JsonFile\\Person.txt";
            string jsonText = File.ReadAllText(filePath);
            return jsonText;
        }

        public void WriteContentOfTheFile(string input) 
        {
            string filePath = "C:\\JsonFile\\Person.txt";
            File.AppendAllText(filePath,input);
        }
    }
}
