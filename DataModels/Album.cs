using FluentCloudMusic.Services;
using FluentCloudMusic.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FluentCloudMusic.DataModels
{
    public class Album : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Album()
        {
            _ID = string.Empty;
            _Name = string.Empty;
            _Description = string.Empty;
            _CoverImageUrl = "ms-appx:///Assets/LargeTile.scale-400.png";
        }

        private string _ID;
        public string ID
        {
            get { return _ID; }
            set
            {
                if (_ID == value) return;
                _ID = value;
                Notify();
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value) return;
                _Name = value;
                Notify();
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description == value) return;
                _Description = value;
                Notify();
            }
        }

        private string _CoverImageUrl;
        public string CoverPictureUrl
        {
            get { return _CoverImageUrl; }
            set
            {
                if (StringUtils.IsSameImageUrl(_CoverImageUrl, value)) return;
                _CoverImageUrl = value;
                Notify();
            }
        }

        public static Album FromJson(JToken data, DataSource source)
        {
            return source switch
            {
                DataSource.Official => ParseOfficialAlbum(data),
                DataSource.User => ParseUserAlbum(data),
                _ => throw new InvalidEnumArgumentException()
            };
        }

        public async Task<(bool IsSuccess, Album albumInfo, LinkedList<Song> songs)> GetDetail()
        {
            var (isSuccess, jsonResult) = await AlbumService.GetAlbumDetailAsync(ID);

            if (!isSuccess) return (false, null, null);

            var album = new Album()
            {
                ID = ID,
                Name = jsonResult["album"]["name"].ToString(),
                Description = jsonResult["album"]["description"].ToString(),
                CoverPictureUrl = jsonResult["album"]["blurPicUrl"].ToString(),
            };

            var songs = new LinkedList<Song>();
            foreach (var item in jsonResult["songs"]) songs.AddLast(Song.FromJson(item, DataSource.Official));

            return (true, album, songs);
        }

        public void CopyTo(Album dest)
        {
            dest.ID = _ID;
            dest.Name = _Name;
            dest.Description = _Description;
            dest.CoverPictureUrl = _CoverImageUrl;
        }

        private static Album ParseOfficialAlbum(JToken data)
        {
            return new Album()
            {
                ID = data["id"].ToString(),
                Name = data["name"].ToString()
            };
        }

        private static Album ParseUserAlbum(JToken data)
        {
            return new Album()
            {
                Name = data["album"].ToString()
            };
        }

        private void Notify([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
