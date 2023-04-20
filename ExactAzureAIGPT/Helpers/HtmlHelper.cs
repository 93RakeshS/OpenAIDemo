using System.Text.RegularExpressions;

namespace ExactAzureAIGPT.Helpers
{
    public static class HtmlHelper
    {
        public static string ReplaceNewLineWithHtmlBreak(this string content)
        {
            Regex regex = new Regex(@"(\n)");
            // Replace new line with <br/> tag    
            return regex.Replace(content, "</br>");
        }

        public static string SanitizeHtml(this string content)
        {
            Regex regex = new Regex(@"(</br>)");
            // Replace new line with <br/> tag    
            return regex.Replace(content, "\n");
        }
    }
}
