using FluentCloudMusic.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace FluentCloudMusic.DataModels
{
    public class Song
    {
        public Song This { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public Album Album { get; private set; }
        public Artists Artists { get; private set; }
        public bool HasCopyright { get; private set; }

        private Song()
        {
            This = this;
        }

        public static Song ParseOfficialMusic(JToken json)
        {
            Song result = new Song
            {
                ID = json["id"].ToString(),
                Name = json["name"].ToString(),
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
                result.Alias = $"( {json["tns"].First} | {json["alia"].First} )";
            else if (hasTrans) 
                result.Alias = $"( {json["tns"].First} )";
            else if (hasAlias) 
                result.Alias = $"( {json["alia"].First} )";

            return result;
        }

        public static Song ParseUserMusic(JToken json)
        {
            Song result = new Song
            {
                ID = json["songId"].ToString(),
                Name = json["songName"].ToString(),
                Alias = $"( {json["fileName"]} )",
                Album = new Album()
                {
                    Name = json["album"].ToString()
                },
                HasCopyright = true
            };

            result.Artists = new Artists();
            result.Artists.AddArtist(null, json["artist"].ToString());
            return result;
        }

        public async Task<MediaPlaybackItem> ToMediaPlaybackItem()
        {
            var (isSuccess, result) = await SongService.GetNeteaseSongUrl(this);
            return isSuccess ? result : null;
        }

        public bool RelateTo(string filter)
        {
            Func<string, bool> predicate = (string s) => s.Contains(filter, System.StringComparison.CurrentCultureIgnoreCase);
            return
                predicate(Name) || 
                predicate(Alias) || 
                predicate(Artists.MainArtist.Name) || 
                predicate(Album.Name);
        }
    }
}
