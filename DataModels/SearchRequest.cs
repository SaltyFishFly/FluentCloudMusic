using System.Collections.Generic;

namespace FluentCloudMusic.DataModels
{
    public enum SearchType
    {
        Music = 1, Album = 10, Artist = 100, Playlist = 1000, User = 1002, MV = 1004, Lyric = 1006, Podcast = 1009, Comprehensive = 1018
    }

    public class SearchRequest
    {
        public int Page { get; private set; }
        public int Capacity { get; }
        public string Keywords { get; }
        public SearchType Type { get; }

        public SearchRequest(string keywords, SearchType type = SearchType.Music, int page = 1, int capacity = 30)
        {
            Page = page;
            Capacity = capacity;
            Keywords = keywords;
            Type = type;
        }

        public SearchRequest Prev()
        {
            if (Page > 1) Page--;
            return this;
        }

        public SearchRequest Next()
        {
            Page++;
            return this;
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "limit", Capacity },
                { "offset", (Page - 1) * Capacity },
                { "keywords", Keywords },
                { "type", Type },
            };
        }
    }
}
