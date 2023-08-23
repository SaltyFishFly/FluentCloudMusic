using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentCloudMusic.Services
{
    public static class AlbumService
    {
        public static async Task<(bool IsSuccess, Album AlbumInfo)> GetAlbumDetailAsync(string albumID)
        {
            var parameters = new Dictionary<string, object> { { "id", albumID } };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Album, parameters);
            var result = jsonResult.ToObject<AlbumResponse>(JsonUtils.Serializer);

            return result.Code == 200 ? (true, result.Album) : (false, null);
        }
    }
}
