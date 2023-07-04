using Newtonsoft.Json;

namespace Devon.Azure.AI.GPT.Tests
{
    static class Utility
    {
        public static string apikey = "4e248eeb1cd9440e8933201836a72bbd";
        public static string apiurl = "https://eolai.openai.azure.com";

        public static List<RequestResponse> GetData(string filepath)
        {
            List<RequestResponse> listRequestResponse = new List<RequestResponse>();
            using (StreamReader r = new StreamReader(filepath))
            {
                string json = r.ReadToEnd();
                listRequestResponse = JsonConvert.DeserializeObject<List<RequestResponse>>(json);
            }
            foreach (RequestResponse reqresponse in listRequestResponse)
            {
                Console.WriteLine("User Request: " + reqresponse.UserRequest);
                Console.WriteLine("User Response: " + reqresponse.UserResponse);
            }
            return listRequestResponse;
        }
        public static string ReadSystemMessage(string filepath)
        {
            string systemMessage = "";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "SystemMessage", "SystemMessage.txt");
            using (StreamReader r = new StreamReader(filepath))
            {
                systemMessage = r.ReadToEnd();
            }
            return systemMessage;
        }
    }
}
