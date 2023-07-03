using System.Text.RegularExpressions;
using System.Web;

namespace Exact.Azure.AI.GPT.Helpers
{
    public static class HtmlHelper
    {
        public static string ReplaceNewLineWithHtmlBreak(this string content)
        {
			Regex regex = new Regex(@"(\n)");
			return regex.Replace(content, "</br>");
		}

        public static string SanitizeHtml(this string content)
        {
            Regex regex = new Regex(@"(</br>)");
            return regex.Replace(content, "\n");
        }
    }
}
