using System.Collections.Generic;

namespace FluentCloudMusic.DataModels
{
    public class SearchSection
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Page { get; set; }

        public SearchSection(int limit = 50, int offset = 0)
        {
            Limit = limit;
            Offset = offset;
            Page = offset / limit + 1;
        }

        public void PrevPage()
        {
            Offset -= Limit;
            Page--;
        }

        public void NextPage()
        {
            Offset += Limit;
            Page++;
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "limit", Limit },
                { "offset", Offset }
            };
        }
    }
}
