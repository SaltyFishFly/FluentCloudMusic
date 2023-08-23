using FluentCloudMusic.Utils;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FluentCloudMusic.DataModels
{
    public class DeprecatedAlbum : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DeprecatedAlbum()
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

        public static DeprecatedAlbum FromJson(JToken data, DataSource source)
        {
            return source switch
            {
                DataSource.Official => ParseOfficialAlbum(data),
                DataSource.User => ParseUserAlbum(data),
                _ => throw new InvalidEnumArgumentException()
            };
        }

        public void CopyTo(DeprecatedAlbum dest)
        {
            dest.ID = _ID;
            dest.Name = _Name;
            dest.Description = _Description;
            dest.CoverPictureUrl = _CoverImageUrl;
        }

        private static DeprecatedAlbum ParseOfficialAlbum(JToken data)
        {
            return new DeprecatedAlbum()
            {
                ID = data["id"].ToString(),
                Name = data["name"].ToString()
            };
        }

        private static DeprecatedAlbum ParseUserAlbum(JToken data)
        {
            return new DeprecatedAlbum()
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
