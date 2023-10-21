using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class Album
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonMultipleProperty("alias", "alia")]
        public string[] Alias { get; set; }

        [JsonProperty("picUrl")]
        public string ImageUrl { get; set; }

        public Artist[] Artists { get; set; }

        public long PublishTime { get; set; }

        public async Task<(Album Info, List<Song> Songs)> GetDetailAsync()
        {
            var parameters = new Dictionary<string, object> { { "id", Id } };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Album, parameters);
            var result = jsonResult.ToObject<AlbumResponse>();
            result.CheckCode();

            return (result.Album, result.Songs.ToList());
        }
    }
}
