using System.ComponentModel;

namespace FluentCloudMusic.DataModels.ViewModels
{
    public class DiscoverPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool RecommendPlaylistsLoaded
        {
            get => _RecommendPlaylistsLoaded;
            set
            {
                if (_RecommendPlaylistsLoaded == value) return;
                _RecommendPlaylistsLoaded = value;
                Notify(nameof(RecommendPlaylistsLoaded));
            }
        }
        public bool RecommendSongsLoaded
        {
            get => _RecommendSongsLoaded;
            set
            {
                if (_RecommendSongsLoaded == value) return;
                _RecommendSongsLoaded = value;
                Notify(nameof(RecommendSongsLoaded));
            }
        }

        private bool _RecommendPlaylistsLoaded;
        private bool _RecommendSongsLoaded;

        private void Notify(string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
