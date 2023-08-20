using FluentCloudMusic.DataModels;
using NeteaseCloudMusicApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCloudMusic.Services
{
    public static class NetworkService
    {
        /// <summary>
        /// 获取日推歌单
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Playlist>> GetDailyRecommendPlaylistsAsync()
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.RecommendResource);

            var result = new List<Playlist>();
            if (jsonResult["code"].Value<int>() == 200)
                foreach (var item in jsonResult["recommend"]) result.Add(Playlist.Parse(item));
            return result;
        }

        /// <summary>
        /// 获取日推单曲
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Song>> GetDailyRecommendSongsAsync()
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.RecommendSongs);

            var result = new List<Song>();
            if (jsonResult["code"].Value<int>() == 200)
                foreach (var item in jsonResult["data"]["dailySongs"]) result.Add(Song.ParseOfficialMusic(item));
            return result;
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="keywords">搜索词</param>
        /// <param name="type">搜索类型</param>
        /// <param name="offset">偏移量</param>
        /// <returns></returns>
        public static async Task<(bool IsSuccess, int CurrentPage, LinkedList<Song> SearchResults)> SearchAsync(SearchRequest Request)
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Cloudsearch, Request.ToDictionary());
            if (jsonResult["code"].Value<int>() == 200 &&
                jsonResult["result"]["songs"] != null)
            {
                var result = new LinkedList<Song>();
                foreach (JToken jsonSearchResult in jsonResult["result"]["songs"])
                {
                    var searchResult = Song.ParseOfficialMusic(jsonSearchResult);
                    result.AddLast(searchResult);
                }
                //计算页数
                int Page = (int)Math.Ceiling(jsonResult["result"]["songCount"].Value<double>() / Request.Section.Limit);
                return (true, Page, result);
            }
            return (false, 0, null);
        }

        /// <summary>
        /// 获取歌单信息和所有歌曲
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>
        public static async Task<(bool IsSuccess, JToken PlaylistInfo, LinkedList<Song> SongList)> GetPlaylistDetailAsync(string playlistID)
        {
            var parameters = new Dictionary<string, object> { { "id", playlistID } };
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.PlaylistDetail, parameters);
            if (jsonResult["code"].Value<int>() == 200)
            {
                StringBuilder musicIDsBuilder = new StringBuilder();
                foreach (var Item in jsonResult["playlist"]["trackIds"])
                {
                    musicIDsBuilder.Append(Item["id"].ToString()).Append(",");
                }
                string MusicIds = musicIDsBuilder.Remove(musicIDsBuilder.Length - 1, 1).ToString();

                var result = new LinkedList<Song>();
                var parameters2 = new Dictionary<string, object> { { "ids", MusicIds } };
                var jsonResult2 = await App.API.RequestAsync(CloudMusicApiProviders.SongDetail, parameters2);
                if (jsonResult2["code"].Value<int>() == 200)
                {
                    foreach (var Item in jsonResult2["songs"])
                    {
                        result.AddLast(Song.ParseOfficialMusic(Item));
                    }
                    return (true, jsonResult["playlist"], result);
                }
                return (false, null, null);
            }
            return (false, null, null);
        }

        public static async Task<(bool IsSuccess, int CurrentPage, LinkedList<Song> SongList)> GetUserCloudAsync(SearchSection section)
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.UserCloud, section.ToDictionary());
            var code = jsonResult["code"].Value<int>();
            if (code == 200)
            {
                var result = new LinkedList<Song>();
                foreach (var item in jsonResult["data"])
                {
                    result.AddLast(Song.ParseUserMusic(item));
                }
                int page = (int)Math.Ceiling(jsonResult["count"].Value<double>() / section.Limit);
                return (true, page, result);
            }
            return (false, 0, null);
        }

        /// <summary>
        /// 获取专辑信息
        /// </summary>
        /// <param name="albumID"></param>
        /// <returns></returns>
        public static async Task<(bool IsSuccess, JToken AlbumInfo)> GetAlbumDetailAsync(string albumID)
        {
            var parameters = new Dictionary<string, object> { { "id", albumID } };
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Album, parameters);
            if (jsonResult["code"].Value<int>() != 200) return (false, null);
            return (true, jsonResult);
        }

        /// <summary>
        /// 获取播放列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<(bool IsSuccess, LinkedList<Playlist>)> GetUserPlaylist(string uid)
        {
            var parameters = new Dictionary<string, object> { { "uid", uid } };
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.UserPlaylist, parameters);
            if (jsonResult["code"].Value<int>() == 200)
            {
                var result = new LinkedList<Playlist>();
                foreach (var item in jsonResult["playlist"])
                {
                    result.AddLast(new Playlist
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