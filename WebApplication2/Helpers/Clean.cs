using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace WebApplication2.Helpers
{
    public static class Clean
    {
        public static string CleanContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            // Remove image tags
            content = Regex.Replace(content, @"<figure[^>]*>.*?</figure>", string.Empty, RegexOptions.Singleline);

            // Remove excessive line breaks
            content = Regex.Replace(content, @"\n{2,}", "\n");

            // Trim and split lines
            content = content.Trim();
            var lines = content.Split('\n');

            // Truncate to 3 lines if needed
            if (lines.Length > 3)
            {
                content = string.Join("\n", lines.Take(3)) + "...";
            }

            return content;
        }
    }
}