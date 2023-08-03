using System.Net;
using System.Text;

namespace FluentNetease.Classes
{
    public static class CookieCollectionExtension
    {
        public static string GetString(this CookieCollection collection)
        {
            StringBuilder cookieBuilder = new StringBuilder();
            foreach (Cookie cookie in collection)
            {
                cookieBuilder
                    .Append(cookie.ToString())
                    .Append(';');
            }
            cookieBuilder.Remove(cookieBuilder.Length - 1, 1);
            return cookieBuilder.ToString();
        }

        public static void LoadFromString(this CookieCollection collection, string str)
        {
            var tokens = str.Split(';');
            foreach (var token in tokens)
            {
                var kvPair = token.Split('=');
                var cookie = new Cookie(kvPair[0], kvPair[1]);
                collection.Add(cookie);
            }
        }
    }
}