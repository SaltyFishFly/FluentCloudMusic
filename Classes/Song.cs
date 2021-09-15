using Newtonsoft.Json.Linq;

namespace FluentNetease.Classes
{
    public class Song
    {
        public AbstractMusic Music { get; set; }
        public Album Album { get; set; }
        public Artists Artists { get; set; }

        private Song() { }

        public static Song ParseOfficialMusic(JToken json)
        {
            Song result = new Song();

            result.Music = new NeteaseMusic()
            {
                ID = json["id"].ToString(),
                Name = json["name"].ToString(),
            };
            result.Album = new Album()
            {
                ID = json["al"]["id"].ToString(),
                Name = json["al"]["name"].ToString()
            };
            result.Artists = new Artists();
            foreach (var Item in json["ar"])
            {
                result.Artists.AddArtist(Item["id"].ToString(), Item["name"].ToString());
            }

            bool hasTrans = json["tns"] != null && json["tns"].HasValues;
            bool hasAlias = json["alia"] != null && json["alia"].HasValues;
            if (hasTrans && hasAlias)
            {
                result.Music.Description = "( " + json["tns"].First.ToString() + " | " + json["alia"].First.ToString() + " )";
            }
            else if (hasTrans)
            {
                result.Music.Description = "( " + json["tns"].First.ToString() + " )";
            }
            else if (hasAlias)
            {
                result.Music.Description = "( " + json["alia"].First.ToString() + " )";
            }

            return result;
        }

        public static Song ParseUserMusic(JToken json)
        {
            Song result = new Song();

            result.Music = new NeteaseMusic()
            {
                ID = json["songId"].ToString(),
                Name = json["songName"].ToString(),
                Description = "( " + json["fileName"].ToString() + " )"
            };
            result.Album = new Album()
            {
                Name = json["album"].ToString()
            };
            result.Artists = new Artists()
            {
                MainArtistName = json["artist"].ToString()
            };

            return result;
        }
    }
}
