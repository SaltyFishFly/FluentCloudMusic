using FluentCloudMusic.DataModels;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentCloudMusic.Services
{
    public static class NetworkService
    {
        // 这个类已过期，不应该再往里添加新函数
        // 遗留的函数应该被拆分到其它Service里

        /// <summary>
        /// 获取日推歌单
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Playlist>> GetDailyRecommendPlaylistsAsync()
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.RecommendResource);
            var result = jsonResult.ToObject<RecommendResourcesResponse>(JsonUtils.Serializer);
            return result.Code == 200 ? result.Playlists.ToList() : new List<Playlist>();
        }

        /// <summary>
        /// 获取日推单曲
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Song>> GetDailyRecommendSongsAsync()
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.RecommendSongs);
            var result = jsonResult.ToObject<RecommendSongsResponse>(JsonUtils.Serializer);
            return result.Code == 200 ? result.Data.DailySongs.ToList() : new List<Song>();
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="keywords">搜索词</param>
        /// <param name="type">搜索类型</param>
        /// <param name="offset">偏移量</param>
        /// <returns></returns>
        public static async Task<(bool IsSuccess, int CurrentPage, List<Song> SearchResults)> SearchAsync(SearchRequest Request)
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Cloudsearch, Request.ToDictionary());
            var result = jsonResult.ToObject<CloudSearchResponse>(JsonUtils.Serializer);

            if (result.Code != 200) return (false, 0, null);

            //计算页数
            int Page = (int)Math.Ceiling(jsonResult["result"]["songCount"].Value<double>() / Request.Section.Limit);
            return (true, Page, result.Result.Songs.ToList());
        }

        /// <summary>
        /// 获取歌单信息和所有歌曲
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>
        public static async Task<(bool IsSuccess, Playlist Info, List<Song> Songs)> GetPlaylistDetailAsync(string playlistID)
        {
            var playlistParams = new Dictionary<string, object> { { "id", playlistID } };

            var jsonResult1 = await App.API.RequestAsync(CloudMusicApiProviders.PlaylistDetail, playlistParams);
            var result1 = jsonResult1.ToObject<PlaylistDetailResponse>(JsonUtils.Serializer);

            if (result1.Code != 200) return (false, null, null);
            if (result1.Playlist.TrackIds.Length == 0) return (true, result1.Playlist, new List<Song>());


            StringBuilder musicIDsBuilder = new StringBuilder();
            foreach (var track in result1.Playlist.TrackIds) musicIDsBuilder.Append(track.Id).Append(",");
            musicIDsBuilder.Remove(musicIDsBuilder.Length - 1, 1);

            var songParams = new Dictionary<string, object> { { "ids", musicIDsBuilder.ToString() } };
            var jsonResult2 = await App.API.RequestAsync(CloudMusicApiProviders.SongDetail, songParams);
            var result2 = jsonResult2.ToObject<SongDetailResponse>(JsonUtils.Serializer);

            if (result2.Code != 200) return (true, result1.Playlist, new List<Song>());

            return (true, result1.Playlist, result2.Songs.ToList());
        }

        public static async Task<(bool IsSuccess, int CurrentPage, LinkedList<DeprecatedSong> SongList)> GetUserCloudAsync(SearchSection section)
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.UserCloud, section.ToDictionary());
            var code = jsonResult["code"].Value<int>();
            if (code == 200)
            {
                var result = new LinkedList<DeprecatedSong>();
                foreach (var item in jsonResult["data"])
                {
                    result.AddLast(DeprecatedSong.FromJson(item, DataSource.User));
                }
                int page = (int)Math.Ceiling(jsonResult["count"].Value<double>() / section.Limit);
                return (true, page, result);
            }
            return (false, 0, null);
        }

        /// <summary>
        /// 获取播放列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<(bool IsSuccess, LinkedList<DeprecatedPlaylist>)> GetUserPlaylist(string uid)
        {
            var parameters = new Dictionary<string, object> { { "uid", uid } };
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.UserPlaylist, parameters);
            if (jsonResult["code"].Value<int>() == 200)
            {
                var result = new LinkedList<DeprecatedPlaylist>();
                foreach (var item in jsonResult["playlist"])
                {
                    result.AddLast(new DeprecatedPlaylist
                    {
                        ID = item["id"].ToString(),
                        Name = item["name"].ToString(),
                        CoverPictureUrl = item["coverImgUrl"].ToString(),
                        CreatorID = item["creator"]["userId"].ToString(),
                        Privacy = item["privacy"].Value<int>()
                    });
                }
                return (true, result);
            }
            return (false, null);
        }
    }
}