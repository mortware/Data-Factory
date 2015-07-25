using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DataFactory
{
    public static partial class Extensions
    {
        public static string GetStringBetweenStrings(this string source, string start, string end)
        {
            string newString;
            if (source.Contains(start) && source.Contains(end))
            {
                int startPos = source.IndexOf(start, StringComparison.Ordinal) + start.Length;
                int endPos = source.IndexOf(end, StringComparison.Ordinal);
                int newStringLength = endPos - startPos;
                newString = source.Substring(startPos, newStringLength);
            }
            else return null;
            return newString;
        }
        public static string RemoveHtml(this string source)
        {
            return Regex.Replace(source, "<.*?>", String.Empty);
        }
        public static string ToProper(this string source)
        {
            var sb = new StringBuilder();

            bool emptyBefore = true;
            foreach (char ch in source)
            {
                char chThis = ch;
                if (Char.IsWhiteSpace(chThis))
                    emptyBefore = true;
                else
                {
                    if (Char.IsLetter(chThis) && emptyBefore)
                        chThis = Char.ToUpper(chThis);
                    else
                        chThis = Char.ToLower(chThis);
                    emptyBefore = false;
                }
                sb.Append(chThis);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Indicates whether this System.String can be converted to a number
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string source)
        {
            float output;
            return float.TryParse(source, out output);
        }
        /// <summary>
        /// Extracts all digits from a string
        /// </summary>
        /// <param name="source"></param>
        /// <returns>All digits contained within the string</returns>
        public static string ExtractDigits(this string source)
        {
            return string.Join(null, Regex.Split(source, "[^\\d]"));
        }
    }
}
