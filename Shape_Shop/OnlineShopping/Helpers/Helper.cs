using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineShopping.Helpers
{
    public static class Helper
    {
        public static string CreateSaltKey(int size)
        {
            // Generate a cryptographic random number
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        public static string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            var saltAndPassword = String.Concat(password, saltkey);
            var hashedPassword =
                FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, passwordFormat);
            return hashedPassword;
        }

        public static MvcHtmlString EncodedMultiLineText(this HtmlHelper helper, string text)
        {
            return String.IsNullOrEmpty(text)
                ? MvcHtmlString.Create(string.Empty)
                : MvcHtmlString.Create(Regex.Replace(helper.Encode(text), Environment.NewLine, "<br />"));
        }

        /// <summary>
        /// Generates a relative textual string based on date in context in relation to current system's date and time.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToRelativeDate(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;
            // span is less than or equal to 60 seconds, measure in seconds.
            if (timeSpan <= TimeSpan.FromSeconds(60))
            {

                return timeSpan.Seconds <= 0 ? "A moment ago" : timeSpan.Seconds + " seconds ago";
            }

            // span is less than or equal to 60 minutes, measure in minutes.

            if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                return timeSpan.Minutes > 1 ? "About " + timeSpan.Minutes + " minutes ago" : "About a minute ago";
            }

            // span is less than or equal to 24 hours, measure in hours.

            if (timeSpan <= TimeSpan.FromHours(24))
            {
                return timeSpan.Hours > 1 ? "About " + timeSpan.Hours + " hours ago at " + String.Format("{0:t}", dateTime) : "About an hour ago";
            }
            // span is less than 48 or greater than 24 hours measure as Yesterday.
            //TODO: At 01:00, something posted at 23:00 the prior day would technically be “yesterday”, but with the current counts would read as “about 2 hours ago…” 
            if (timeSpan <= TimeSpan.FromHours(48))
            {
                return "Yesterday at " + String.Format("{0:t}", dateTime);
            }

            // span is less than or equal to 4 days, measure in days.

            if (timeSpan <= TimeSpan.FromDays(4))
            {
                return timeSpan.Days > 1 ? "About " + timeSpan.Days + " days ago" + " (" + String.Format("{0:f}", dateTime) + ")" : "About a day ago" + " (" + String.Format("{0:f}", dateTime) + ")";
            }

            // span is less than or equal to 30 days (1 month), measure in days.

            //if (timeSpan <= TimeSpan.FromDays(30))
            //{
            //    return timeSpan.Days > 1 ? "about " + timeSpan.Days + " days ago" + " (" + String.Format("{0:f}", dateTime) + ")" : "about a day ago" + " (" + String.Format("{0:f}", dateTime) + ")";
            //}
            // span is less than or equal to 365 days (1 year), measure in months.

            //if (timeSpan <= TimeSpan.FromDays(365))
            //{
            //    return timeSpan.Days > 30 ? "about " + timeSpan.Days / 30 + " months ago" : "about a month ago";
            //}
            //// span is greater than 365 days (1 year), measure in years.
            //return timeSpan.Days > 365 ? "about " + timeSpan.Days / 365 + " years ago" : "about a year ago";
            return String.Format("{0:f}", dateTime);
        }

        /// <summary>
        /// Substring but OK if shorter
        /// </summary>
        private static string Limit(this string str, int characterCount)
        {
            return str.Length <= characterCount ? str : str.Substring(0, characterCount).TrimEnd(' ');
        }

        /// <summary>
        /// Substring with elipses but OK if shorter, will take 3 characters off character count if necessary
        /// </summary>
        public static string LimitWithElipses(this string str, int characterCount)
        {
            return characterCount < 5
                ? str.Limit(characterCount)
                : (str.Length <= characterCount - 3 ? str : str.Substring(0, characterCount - 3) + "…");
        }

        /// <summary>
        /// Substring with elipses but OK if shorter, will take 3 characters off character count if necessary
        /// tries to land on a space.
        /// </summary>
        public static string LimitWithElipsesOnWordBoundary(this string str, int characterCount)
        {
            if (characterCount < 5) return str.Limit(characterCount);       // Can’t do much with such a short limit
            if (str.Length <= characterCount - 3)
                return str;
            var lastspace = str.Substring(0, characterCount - 3).LastIndexOf(' ');
            if (lastspace > 0 && lastspace > characterCount - 10)
                return str.Substring(0, lastspace) + "…";
            // No suitable space was found
            return str.Substring(0, characterCount - 3) + "…";
        }
    }
}