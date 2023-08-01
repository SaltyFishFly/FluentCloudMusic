using NeteaseCloudMusicApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace FluentNetease.Classes
{
    class Network
    {
        /// <summary>
        /// 登录(目前只支持手机)
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>(Code, JObject) 返回代码和JSON对象</returns>
        public static async Task<(int Code, JObject Result)> LoginAsync(string countryCode, string account, string password)
        {
            var Parameters = new Dictionary<string, object>
                {
                    { "countrycode", countryCode},
                    { "phone", account },
                    { "password", password }
                };
            var RequestResult = await App.API.RequestAsync(CloudMusicApiProviders.LoginCellphone, Parameters);
            var RequestCode = RequestResult["code"].Value<int>();
            return (RequestCode, RequestResult);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> LogoutAsync()
        {
            var RequestResult = await App.API.RequestAsync(CloudMusicApiProviders.Logout);
            if (RequestResult["code"].Value<int>() == 200)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取日推
        /// </summary>
        /// <returns></returns>
        public static async Task<LinkedList<Playlist>> GetDailyRecommendPlaylistAsync()
        {
            var RequestResult = await App.API.RequestAsync(CloudMusicApiProviders.RecommendResource);
            if (RequestResult["code"].Value<int>() == 200)
            {
                var Result = new LinkedList<Playlist>();
                foreach (var Item in RequestResult["recommend"])
                {
                    var Playlist = new Playlist
                    {
                        ID = Item["id"].ToString(),
                        Name = Item["name"].ToString(),
                        CoverPictureUrl = Item["picUrl"].ToString(),
                        CreatorID = Item["creator"]["userId"].ToString(),
                        Privacy = 0
                    };
                    Result.AddLast(Playlist);
                }
                return Result;
            }
            return null;
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
            var RequestResult = await App.API.RequestAsync(CloudMusicApiProviders.Cloudsearch, Request.ToDictionary());
            if (RequestResult["code"].Value<int>() == 200 &&
                RequestResult["result"]["songs"] != null)
            {
                var Result = new LinkedList<Song>();
                foreach (JToken JsonSearchResult in RequestResult["result"]["songs"])
                {
                    var SearchResult = Song.ParseOfficialMusic(JsonSearchResult);
                    Result.AddLast(SearchResult);
                }
                //计算页数
                int Page = (int)Math.Ceiling(RequestResult["result"]["songCount"].Value<double>() / Request.Section.Limit);
                return (true, Page, Result);
            }
            return (false, 0, null);
        }

        /// <summary>
        /// 获取音乐播放地址
        /// </summary>
        /// <param name="musicId"></param>
        /// <returns></returns>
        public static async Task<(bool IsSuccess, MediaPlaybackItem Result)> GetOfficialMusicUrlAsync(string musicId)
        {
            var Parameters = new Dictionary<string, object> { { "id", musicId } };
            var RequestResult = await App.API.RequestAsync(CloudMusicApiProviders.SongUrl, Parameters);
            if (RequestResult["code"].Value<int>() == 200 &&
                RequestResult["data"].First["url"].ToString() != string.Empty)
            {
                var Result = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(RequestResult["data"].First["url"].ToString())));
                return (true, Result);
            }
            return (false, null);
        }

        /// <summary>
        /// 获取歌单信息和所有歌曲
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>
        public static async Task<(bool IsSuccess, JToken PlaylistInfo, LinkedList<Song> SongList)> GetPlaylistDetailAsync(string playlistID)
        {
            var Parameters = new Dictionary<string, object> { { "id", playlistID } };
            var RequestResult = await App.API.RequestAsync(CloudMusicApiProviders.PlaylistDetail, Parameters);
            if (RequestResult["code"].Value<int>() == 200)
            {
                StringBuilder MusicIdsBuilder = new StringBuilder();
                foreach (var Item in RequestResult["playlist"]["trackIds"])
                {
                    MusicIdsBuilder.Append(Item["id"].ToString()).Append(",");
                }
                string MusicIds = MusicIdsBuilder.Remove(MusicIdsBuilder.Length - 1, 1).ToString();

                var Result = new LinkedList<Song>();
                var Parameters2 = new Dictionary<string, object> { { "ids", MusicIds } };
                var RequestResult2 = await App.API.RequestAsync(CloudMusicApiProviders.SongDetail, Parameters2);
                if (RequestResult2["code"].Value<int>() == 200)
                {
                    foreach (var Item in RequestResult2["songs"])
                    {
                        Result.AddLast(Song.ParseOfficialMusic(Item));
                    }
                    return (true, RequestResult["playlist"], Result);
                }
                return (false, null, null);
            }
            return (false, null, null);
        }

        public static async Task<(bool IsSuccess, int CurrentPage, LinkedList<Song> SongList)> GetUserCloudAsync(SearchSection section)
        {
            var RequestResult = await App.API.RequestAsync(CloudMusicApiProviders.UserCloud, section.ToDictionary());
            if (RequestResult["code"].Value<int>() == 200)
            {
                var Result = new LinkedList<Song>();
                foreach (var Item in RequestResult["data"])
                {
                    Result.AddLast(Song.ParseUserMusic(Item));
                }
                int Page = (int)Math.Ceiling(RequestResult["count"].Value<double>() / section.Limit);
                return (true, Page, Result);
            }
            return (false, 0, null);
        }

        /// <summary>
        /// 获取专辑信息
        /// </summary>
        /// <param name="albumID"></param>
        /// <returns></returns>
        public static async Task<(bool IsSuccess, JToken AlbumInfo, LinkedList<Song> Result)> GetAlbumDetailAsync(string albumID)
        {
            var Parameters = new Dictionary<string, object> { { "id", albumID } };
            var RequestResult = await App.API.RequestAsync(CloudMusicApiProviders.Album, Parameters);
            if (RequestResult["code"].Value<int>() == 200)
            {
                var Result = new LinkedList<Song>();
                foreach (var Item in RequestResult["songs"])
                {
                    Result.AddLast(Song.ParseOfficialMusic(Item));
                }
                return (true, RequestResult["album"], Result);
            }
            return (false, null, null);
        }

        /// <summary>
        /// 获取播放列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<(bool IsSuccess, LinkedList<Playlist>)> GetUserPlaylist(string uid)
        {
            var Parameters = new Dictionary<string, object> { { "uid", uid } };
            var RequestResult = await App.API.RequestAsync(CloudMusicApiProviders.UserPlaylist, Parameters);
            if (RequestResult["code"].Value<int>() == 200)
            {
                var Result = new LinkedList<Playlist>();
                foreach (var Item in RequestResult["playlist"])
                {
                    Result.AddLast(new Playlist
                    {
                        ID = Item["id"].ToString(),
                        Name = Item["name"].ToString(),
                        CoverPictureUrl = Item["coverImgUrl"].ToString(),
                        CreatorID = Item["creator"]["userId"].ToString(),
                        Privacy = Item["privacy"].Value<int>()
                    });
                }
                return (true, Result);
            }
            return (false, null);
        }
    }
}