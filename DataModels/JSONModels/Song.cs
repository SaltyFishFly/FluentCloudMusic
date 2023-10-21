using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Services;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class Song : ISong
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] Alias { get; set; }

        [JsonProperty("ar")]
        public Artist[] Artists { get; set; }

        [JsonProperty("al")]
        public Album Album { get; set; }

        [JsonProperty("noCopyrightRcmd")]
        public object NoCopyrightRecommendation { get; set; }

        [JsonProperty("hr")]
        public QualityInfo HiResQuality { get; set; }

        [JsonProperty("sq")]
        public QualityInfo SuperQuality { get; set; }

        [JsonProperty("h")]
        public QualityInfo HighQuality { get; set; }

        [JsonProperty("m")]
        public QualityInfo MediumQuality { get; set; }

        [JsonProperty("l")]
        public QualityInfo LowQuality { get; set; }

        [JsonProperty("tns")]
        public string[] Translations { get; set; }


        public bool HasCopyright { get => NoCopyrightRecommendation == null; }

        public string ImageUrl => Album.ImageUrl ?? "ms-appx:///Assets/LargeTile.scale-400.png";

        public Artist MainArtist { get => Artists[0]; }

        public string AlbumName { get => Album.Name; }

        public string ArtistName
        {
            get
            {
                if (Artists.Length == 0) return string.Empty;

                var result = new StringBuilder();
                Array.ForEach(Artists, artist => result.Append(artist.Name).Append(" / "));
                result.Remove(result.Length - 3, 3);

                return result.ToString();
            }
        }
        public string Description
        {
            get
            {
                bool hasAlias = Alias != null && Alias.Length > 0;
                bool hasTrans = Translations != null && Translations.Length > 0;

                if (hasTrans && hasAlias)
                    return $"( {Translations.First()} | {Alias.First()} )";
                else if (hasTrans)
                    return $"( {Translations.First()} )";
                else if (hasAlias)
                    return $"( {Alias.First()} )";

                return string.Empty;
            }
        }

        public bool RelateTo(string filter)
        {
            bool predicate(string s) => s.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
            return
                predicate(Name) ||
                predicate(Description) ||
                predicate(ArtistName) ||
                predicate(AlbumName);
        }

        public async Task<MediaPlaybackItem> ToMediaPlaybackItem()
        {
            var result = await SongService.GetNeteaseSongUrl(this);
            return result;
        }
    }

    public class QualityInfo
    {
        [JsonProperty("br")]
        public int BitRate { get; set; }

        public int Fid { get; set; }

        public int Size { get; set; }

        public float Vd { get; set; }

        public int Sr { get; set; }
    }
}
