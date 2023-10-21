using FluentCloudMusic.DataModels.JSONModels.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Windows.Media.Playback;
using Windows.Storage.Streams;

namespace FluentCloudMusic.Utils
{
    public static class ListExtension
    {
        public static List<ISong> Shuffle(this List<ISong> list, int fixedIndex)
        {
            if (list.Count < 2) return new List<ISong>(list);

            var random = new Random();
            var result = new List<ISong>(list);
            var maxIndex = result.Count - 1;

            (result[maxIndex], result[fixedIndex]) = (result[fixedIndex], result[maxIndex]);
            for (int i = result.Count - 2; i > 0; i--)
            {
                int j = random.Next(0, i);
                (result[i], result[j]) = (result[j], result[i]);
            }
            (result[maxIndex], result[fixedIndex]) = (result[fixedIndex], result[maxIndex]);

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

    public static class MediaPlaybackItemExtension
    {
        public static void SetMetadata(this MediaPlaybackItem item, ISong song)
        {
            var metadata = item.GetDisplayProperties();

            metadata.Type = Windows.Media.MediaPlaybackType.Music;
            metadata.MusicProperties.Title = $"{song.Name}{song.Description}";
            metadata.MusicProperties.Artist = song.ArtistName;
            metadata.MusicProperties.AlbumTitle = song.AlbumName;
            metadata.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(song.ImageUrl));

            item.ApplyDisplayProperties(metadata);
        }
    }
}
