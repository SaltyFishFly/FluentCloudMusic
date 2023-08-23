using FluentCloudMusic.Services;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace FluentCloudMusic.DataModels
{
    public class DeprecatedSong
    {
        public DeprecatedSong This { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public DeprecatedAlbum Album { get; private set; }
        public DrprecatedArtists Artists { get; private set; }
        public bool HasCopyright { get; private set; }

        private DeprecatedSong()
        {
            This = this;
            ID = string.Empty;
            Name = string.Empty;
            Alias = string.Empty;
            HasCopyright = false;
        }

        public static DeprecatedSong FromJson(JToken data, DataSource source)
        {
            return source switch
            {
                DataSource.Official => ParseOfficialMusic(data),
                DataSource.User => ParseUserMusic(data),
                _ => throw new InvalidEnumArgumentException()
            };
        }

        public async Task<MediaPlaybackItem> ToMediaPlaybackItem()
        {
            var (isSuccess, result) = await SongService.GetNeteaseSongUrl(this);
            return isSuccess ? result : null;
        }

        public bool RelateTo(string filter)
        {
            bool predicate(string s) => s.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
            return
                predicate(Name) ||
                predicate(Alias) ||
                predicate(Artists.MainArtist.Name) ||
                predicate(Album.Name);
        }

        private static DeprecatedSong ParseOfficialMusic(JToken data)
        {
            DeprecatedSong result = new DeprecatedSong
            {
                ID = data["id"].ToString(),
                Name = data["name"].ToString(),
                Album = DeprecatedAlbum.FromJson(data["al"], DataSource.Official),
                HasCopyright = !data["noCopyrightRcmd"].HasValues
            };

            result.Artists = new DrprecatedArtists();
            foreach (var artist in data["ar"]) result.Artists.Add(artist["id"].ToString(), artist["name"].ToString());

            bool hasTrans = data["tns"] != null && data["tns"].HasValues;
            bool hasAlias = data["alia"] != null && data["alia"].HasValues;

            if (hasTrans && hasAlias)
                result.Alias = $"( {data["tns"].First} | {data["alia"].First} )";
            else if (hasTrans) 
                result.Alias = $"( {data["tns"].First} )";
            else if (hasAlias) 
                result.Alias = $"( {data["alia"].First} )";

            return result;
        }

        private static DeprecatedSong ParseUserMusic(JToken data)
        {
            DeprecatedSong result = new DeprecatedSong
            {
                ID = data["songId"].ToString(),
                Name = data["songName"].ToString(),
                Alias = $"( {data["fileName"]} )",
                Album = DeprecatedAlbum.FromJson(data, DataSource.User),
                Artists = new DrprecatedArtists()
                {
                    { string.Empty, data["artist"].ToString() }
                },
                HasCopyright = true
            };

            return result;
        }
    }
}
