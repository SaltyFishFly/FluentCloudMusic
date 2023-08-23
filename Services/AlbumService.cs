using NeteaseCloudMusicApi;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentCloudMusic.Services
{
    public static class AlbumService
    {
        public static async Task<(bool IsSuccess, JToken AlbumInfo)> GetAlbumDetailAsync(string albumID)
        {
            var parameters = new Dictionary<string, object> { { "id", albumID } };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Album, parameters);
            var code = jsonResult["code"].Value<int>();

            return code == 200 ? (true, jsonResult) : (false, null);
        }
    }
}
