using System.Collections.Generic;

namespace FluentNetease.Classes
{
    public class SearchRequest
    {
        public string Keywords { get; }
        public SearchType Type { get; }
        public int Limit { get; }
        public int Offset { get; }
        public int Page { get; }

        public SearchRequest(string keywords, SearchType type = SearchType.Music, int limit = 30, int offset = 0)
        {
            Keywords = keywords;
            Type = type;
            Limit = limit;
            Offset = offset;
            Page = (Offset / Limit) + 1;
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "keywords", Keywords },
                { "type", Type },
                { "limit", Limit },
                { "offset", Offset }
            };
        }

        public SearchRequest PrevPage()
        {
            return new SearchRequest(Keywords, Type, Limit, Offset - Limit);
        }

        public SearchRequest NextPage()
        {
            return new SearchRequest(Keywords, Type, Limit, Offset + Limit);
        }

        public enum SearchType
        {
            Music = 1, Album = 10, Artist = 100, Playlist = 1000, User = 1002, MV = 1004, Lyric = 1006, Podcast = 1009, Comprehensive = 1018
        }
    }
}
