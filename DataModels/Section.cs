using System.Collections.Generic;

namespace FluentCloudMusic.DataModels
{
    public class Section
    {
        public int Page { get; private set; }
        public int Capacity { get; }

        public Section(int page = 1, int capacity = 30)
        {
            Page = page;
            Capacity = capacity;
        }

        public Section Prev()
        {
            if (Page > 1) Page--;
            return this;
        }

        public Section Next()
        {
            Page++;
            return this;
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "limit", Capacity },
                { "offset", (Page - 1) * Capacity }
            };
        }
    }
}
