using FluentCloudMusic.DataModels;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentCloudMusic.Services
{
    public static class NetworkService
    {
        // 这个类已过期，不应该再往里添加新函数
        // 遗留的函数应该被拆分到其它Service里

        public static async Task<(bool IsSuccess, int PageCount, List<Song> SearchResults)> SearchAsync(SearchRequest request)
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Cloudsearch, request.ToDictionary());
            var result = jsonResult.ToObject<CloudSearchResponse>(JsonUtils.Serializer);

            if (result.Code != 200) return (false, 0, null);

            int page = 1 + ((result.Result.SongCount - 1) / request.Capacity);

            return (true, page, result.Result.Songs.ToList());
        }

        public static async Task<(bool IsSuccess, int PageCount, List<UserCloudSong> SongList)> GetUserCloudAsync(Section section)
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.UserCloud, section.ToDictionary());
            var result = jsonResult.ToObject<UserCloudResponse>(JsonUtils.Serializer);

            if (result.Code != 200) return (false, 0, null);

            int page = 1 + ((result.Count - 1) / section.Capacity);

            return (true, page, result.Data.ToList());
        }
    }
}