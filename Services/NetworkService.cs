using FluentCloudMusic.DataModels;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using NeteaseCloudMusicApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace FluentCloudMusic.Services
{
    /// <summary>
    /// 这个类已过期，不应该再添加新函数。
    /// </summary>
    [Deprecated("已过期", DeprecationType.Deprecate, 1)]
    public static class NetworkService
    {
        public static async Task<(int PageCount, List<Song> SearchResults)> SearchAsync(SearchRequest request)
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Cloudsearch, request.ToDictionary());
            var result = jsonResult.ToObject<CloudSearchResponse>();
            result.CheckCode();

            int page = 1 + ((result.Result.SongCount - 1) / request.Capacity);

            if (result.Result.SongCount == 0) return (0, new List<Song>());
            return (page, result.Result.Songs.ToList());
        }

        public static async Task<(int PageCount, List<UserCloudSong> SongList)> GetUserCloudAsync(Section section)
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.UserCloud, section.ToDictionary());
            var result = jsonResult.ToObject<UserCloudResponse>();
            result.CheckCode();

            int page = 1 + ((result.Count - 1) / section.Capacity);

            if (result.Count == 0) return (0, new List<UserCloudSong>());
            return (page, result.Data.ToList());
        }
    }
}