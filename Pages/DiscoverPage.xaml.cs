using FluentCloudMusic.Classes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentCloudMusic.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DiscoverPage : Page
    {
        public readonly ObservableCollection<Playlist> DailyRecommendPlaylists;
        public readonly ObservableCollection<Song> DailyRecommendSongs;

        public DiscoverPage()
        {
            DailyRecommendPlaylists = new ObservableCollection<Playlist>();
            DailyRecommendSongs = new ObservableCollection<Song>();

            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetRecommendations();
        }

        private void PlaylistItem_Click(object sender, ItemClickEventArgs e)
        {
            MainPage.Navigate(typeof(PlaylistPage), (Playlist)e.ClickedItem, null);
        }

        private async void GetRecommendations()
        {
            if (!Account.User.HasLogin) return;

            DailyRecommendPlaylists.Clear();
            DailyRecommendSongs.Clear();

            var playlists = await Network.GetDailyRecommendPlaylistsAsync();
            var songs = await Network.GetDailyRecommendSongsAsync();

            playlists?.ForEach(playlist => DailyRecommendPlaylists.Add(playlist));
            songs?.GetRange(0, 5).ForEach(song => DailyRecommendSongs.Add(song));

            MusicList.ApplyFilter(string.Empty);
        }
    }
}