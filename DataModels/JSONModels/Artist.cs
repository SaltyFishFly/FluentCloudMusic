using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class Artist
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] Alias { get; set; }

        [JsonProperty("briefDesc")]
        public string Description { get; set; }

        [JsonMultipleProperty("transNames", "trans", "tns")]
        public string[] Translations { get; set; }

        [JsonMultipleProperty("picUrl", "avatar")]
        public string ImageUrl { get; set; }

        public async Task<Artist> GetDetailAsync()
        {
            var parameters = new Dictionary<string, object> { { "id", Id } };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.ArtistDetail, parameters);
            var result = jsonResult.ToObject<ArtistDetailResponse>();
            result.CheckCode();

            return result.Data.Artist;
        }
    }
}
