using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FluentCloudMusic.DataModels
{
    public class Artist : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Artist()
        {
            _ID = string.Empty;
            _Name = string.Empty;
            _Description = string.Empty;
            _AvatarUrl = "ms-appx:///Assets/LargeTile.scale-400.png";
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

        private string _AvatarUrl;
        public string AvatarUrl
        {
            get { return _AvatarUrl; }
            set
            {
                if (_AvatarUrl == value) return;
                _AvatarUrl = value;
                Notify();
            }
        }

        private void Notify([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }

    public class Artists
    {
        private readonly List<Artist> _Artists;

        public Artist MainArtist { get => _Artists.FirstOrDefault(); }

        public Artists()
        {
            _Artists = new List<Artist>();
        }

        public void AddArtist(string id, string name)
        {
            _Artists.Add(new Artist
            {
                ID = id,
                Name = name
            });
        }
    }
}
