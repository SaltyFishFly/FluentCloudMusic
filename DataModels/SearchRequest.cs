using System.Collections.Generic;

namespace FluentCloudMusic.DataModels
{
    public class SearchRequest
    {
        public string Keywords { get; }
        public SearchType Type { get; }
        public SearchSection Section { get; }

        public SearchRequest(string keywords, SearchType type = SearchType.Music, int limit = 50, int offset = 0)
        {
            Keywords = keywords;
            Type = type;
            Section = new SearchSection(limit, offset);
        }

        public Dictionary<string, object> ToDictionary()
        {
            var Dic = Section.ToDictionary();
            Dic.Add("keywords", Keywords);
            Dic.Add("type", Type);
            return Dic;
        }

        public SearchRequest PrevPage()
        {
            Section.PrevPage();
            return this;
        }

        public SearchRequest NextPage()
        {
            Section.NextPage();
            return this;
        }

        public enum SearchType
        {
            Music = 1, Album = 10, Artist = 100, Playlist = 1000, User = 1002, MV = 1004, Lyric = 1006, Podcast = 1009, Comprehensive = 1018
        }
    }
}
