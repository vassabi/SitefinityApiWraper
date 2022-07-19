using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SitefinityWebApp.Services.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveTrailingSlash(this string value)
        {
            if (!value.IsNullOrWhitespace() && value.EndsWith("/"))
                return value.TrimEnd('/');

            return value;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string TruncateWithTrailingElipse(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : $"{value.Substring(0, maxLength).Trim()}...";
        }

        public static string NullToString(this object value)
        {
            return value == null ? "" : value.ToString();
        }

        public static string TruncateAtWord(string input, int length)
        {
            if (input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ", length);
            return string.Format("{0}...", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }

        public static string CreateUrlName(this string input)
        {
            return Regex.Replace(input.Replace("'", "").Replace("\"", "").Replace("‘", "").Replace("’", "").Replace("“", "").Replace("”", "").ToLower(), RegexCreateUrlName, "-");
        }

        public static string RemoveSpecialCharacters(this string input)
        {
            return input == null ? String.Empty : Regex.Replace(input, "[^0-9a-zA-Z]+", "");
        }

        public static string RemoveNonWhitespaceSpecialCharacters(this string input)
        {
            return input == null ? String.Empty : Regex.Replace(input, @"[^0-9a-zA-Z-_\: ]+", "");
        }

        public static string GetAppConfigurationSetting(this string appSetting)
        {
            return ConfigurationManager.AppSettings[appSetting];
        }

        public static Regex RegexStripHtml = new Regex("<[^>]*>", RegexOptions.Compiled);

        public static string RegexCreateUrlName = @"[^A-Za-z0-9\-\d_]+";

        public static string GetGoogleMapsUrl(this string streetAddress, string city, string state, string zip)
        {
            return string.Format("https://maps.google.com/maps?daddr={0},+{1},+{2},+{3}", streetAddress, city, state, zip);
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static int GetWordCount(this string value)
        {
            return value.Count(Char.IsWhiteSpace);
        }


        /// <summary>
        /// Imperfect of course. Probably needs phonetic input rather than just spelling
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string TryGetIndefiniteArticleFromWord(this string word)
        {
            string article = "a";
            string wordLowered = word.ToLower();

            if (!wordLowered.IsNullOrWhitespace())
            {
                if (wordLowered[0] == 'a' || wordLowered[0] == 'e' || wordLowered[0] == 'i' || wordLowered[0] == 'o')
                {
                    article = "an";
                }
                else if (wordLowered.Count() > 3 && (wordLowered.StartsWith("hon") || wordLowered.IndexOf("i") == 2))
                {
                    article = "an";
                }
            }

            return article;
        }
    }

}