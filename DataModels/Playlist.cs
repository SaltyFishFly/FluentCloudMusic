using FluentCloudMusic.Utils;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FluentCloudMusic.DataModels
{
    public class Playlist : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        private string _CreatorID;
        public string CreatorID
        {
            get { return _CreatorID; }
            set
            {
                if (_CreatorID == value) return;
                _CreatorID = value;
                Notify();
            }
        }

        private int _Privacy;
        public int Privacy
        {
            get { return _Privacy; }
            set
            {
                if (_Privacy == value) return;
                _Privacy = value; 
                Notify(); 
            }
        }

        public Playlist()
        {
            _ID = string.Empty;
            _Name = string.Empty;
            _Description = string.Empty;
            _CoverImageUrl = "ms-appx:///Assets/LargeTile.scale-400.png";
            _CreatorID = string.Empty;
            _Privacy = 0;
        }

        public static Playlist Parse(JToken data)
        {
            return new Playlist
            {
                _ID = data["id"].ToString(),
                _Name = data["name"].ToString(),
                _CoverImageUrl = data["picUrl"].ToString(),
                _CreatorID = data["creator"]["userId"].ToString(),
                _Privacy = 0
            };
        }

        private void Notify([CallerMemberName]string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
