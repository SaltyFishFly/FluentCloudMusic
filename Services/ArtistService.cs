using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentCloudMusic.Services
{
    public static class ArtistService
    {
        public static async Task<(bool isSuccess, Artist artistInfo)> GetArtistDetailAsync(string artistId)
        {
            var parameters = new Dictionary<string, object> { { "id", artistId } };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.ArtistDetail, parameters);
            var result = jsonResult.ToObject<ArtistDetailResponse>(JsonUtils.Serializer);

            return result.Code == 200 ? (true, result.Data.Artist) : (false, null);
        }
    }
}
