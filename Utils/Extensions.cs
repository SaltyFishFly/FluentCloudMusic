using FluentCloudMusic.DataModels;
using FluentCloudMusic.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace FluentCloudMusic.Utils
{
    public static class ListExtension
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            Random random = new Random();

            var result = new List<T>(list);
            for (int i = result.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i);
                (result[i], result[j]) = (result[j], result[i]);
            }
            return result;
        }
    }

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
