using FluentCloudMusic.Classes;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.Services;
using System.Collections.ObjectModel;
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

        public readonly ObservableCollection<Playlist> RecommendPlaylists = new ObservableCollection<Playlist>();
        public readonly ObservableCollection<Song> RecommendSongs = new ObservableCollection<Song>();

        private bool _RecommendPlaylistsLoaded;
        private bool _RecommendSongsLoaded;

        public async void LoadContents()
        {
            RecommendPlaylists.Clear();
            RecommendSongs.Clear();

            try
            {
                var playlists = await PlaylistService.GetDailyRecommendPlaylistsAsync();
                var songs = await SongService.GetDailyRecommendSongsAsync();

                playlists?.ForEach(playlist => RecommendPlaylists.Add(playlist));
                RecommendPlaylistsLoaded = true;

                songs?.GetRange(0, 5).ForEach(song => RecommendSongs.Add(song));
                RecommendSongsLoaded = true;
            }
            catch (ResponseCodeErrorException) { }
        }

        private void Notify(string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
