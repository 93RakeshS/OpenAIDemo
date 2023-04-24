
namespace ExactAzureAIGPT.Services
{
    using ILoggerAlias = ExactAzureAIGPT.Interface.ILogger;
    public class FileLogger : ILoggerAlias
    {
        private string _logDirectory;

        public FileLogger(string logDirectory)
        {
            _logDirectory = logDirectory;
        }

        public void LogInfo(string message)
        {
            try
            {
                string logFilePath = Path.Combine(_logDirectory, $"eolgpt_{DateTime.Now:yyyy-MM-dd}.log");
                string logMessage = $"{DateTime.Now:T} [Info]: {message}";

                // Append the log message to the text file
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(logMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LogError(string message)
        {
            try
            {
                string logFilePath = Path.Combine(_logDirectory, $"eolgpt_{DateTime.Now:yyyy-MM-dd}.log");
                string logMessage = $"{DateTime.Now:T} [Error]: {message}";

                // Append the log message to the text file
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(logMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
