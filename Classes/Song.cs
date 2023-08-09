using Newtonsoft.Json.Linq;

namespace FluentCloudMusic.Classes
{
    public class Song
    {
        public AbstractMusic Music { get; private set; }
        public Album Album { get; private set; }
        public Artists Artists { get; private set; }
        public bool HasCopyright { get; private set; }

        private Song() { }

        public static Song ParseOfficialMusic(JToken json)
        {
            Song result = new Song
            {
                Music = new NeteaseMusic()
                {
                    ID = json["id"].ToString(),
                    Name = json["name"].ToString(),
                },
                Album = new Album()
                {
                    ID = json["al"]["id"].ToString(),
                    Name = json["al"]["name"].ToString()
                },
                Artists = new Artists()
            };
            foreach (var Item in json["ar"])
            {
                result.Artists.AddArtist(Item["id"].ToString(), Item["name"].ToString());
            }
            result.HasCopyright = !json["noCopyrightRcmd"].HasValues;

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
            Song result = new Song
            {
                Music = new NeteaseMusic()
                {
                    ID = json["songId"].ToString(),
                    Name = json["songName"].ToString(),
                    Description = "( " + json["fileName"].ToString() + " )"
                },
                Album = new Album()
                {
                    Name = json["album"].ToString()
                },
                Artists = new Artists()
                {
                    MainArtistName = json["artist"].ToString()
                },
                HasCopyright = true
            };

            return result;
        }
    }
}
