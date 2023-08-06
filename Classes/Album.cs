using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FluentNetease.Classes
{
    public class Album : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Album()
        {
            _ID = null;
            _Name = null;
            _Description = null;
            _CoverPictureUrl = "ms-appx:///Assets/LargeTile.scale-400.png";
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

        private string _CoverPictureUrl;
        public string CoverPictureUrl
        {
            get { return _CoverPictureUrl; }
            set
            {
                if (_CoverPictureUrl == value) return;
                _CoverPictureUrl = value;
                Notify();
            }
        }

        private void Notify([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
