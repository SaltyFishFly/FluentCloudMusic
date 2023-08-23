using FluentCloudMusic.Utils;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FluentCloudMusic.DataModels
{
    public class DrprecatedArtist : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DrprecatedArtist()
        {
            _ID = string.Empty;
            _Name = string.Empty;
            _Description = string.Empty;
            _AvatarImageUrl = "ms-appx:///Assets/LargeTile.scale-400.png";
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

        private string _AvatarImageUrl;
        public string AvatarUrl
        {
            get { return _AvatarImageUrl; }
            set
            {
                if (StringUtils.IsSameImageUrl(_AvatarImageUrl, value)) return;
                _AvatarImageUrl = value;
                Notify();
            }
        }

        private void Notify([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }

    public class DrprecatedArtists : IEnumerable<DrprecatedArtist>
    {
        public DrprecatedArtist MainArtist { get => _Artists.FirstOrDefault(); }

        private readonly List<DrprecatedArtist> _Artists;

        public DrprecatedArtists()
        {
            _Artists = new List<DrprecatedArtist>();
        }

        public override string ToString()
        {
            if (_Artists.Count == 0) return string.Empty;

            var result = new StringBuilder();
            _Artists.ForEach(artist =>  result.Append(artist.Name).Append(" / "));
            result.Remove(result.Length - 3, 3);

            return result.ToString();
        }

        public void Add(string id, string name)
        {
            _Artists.Add(new DrprecatedArtist
            {
                ID = id,
                Name = name
            });
        }

        public IEnumerator<DrprecatedArtist> GetEnumerator()
        {
            return ((IEnumerable<DrprecatedArtist>)_Artists).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_Artists).GetEnumerator();
        }
    }
}
