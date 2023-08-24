using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.Services;
using FluentCloudMusic.Utils;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
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
            var imageContainer = 
                VisualTreeUtils.FindChildByName(DailyRecommendPlaylistsGridView.ContainerFromItem(e.ClickedItem), "CoverImageContainer");
            ConnectedAnimationService
                .GetForCurrentView()
                .PrepareToAnimate("DailyRecommendPlaylistsToPlaylistPageAnimation", (UIElement)imageContainer);
            MainPage.Navigate(typeof(PlaylistPage), (Playlist)e.ClickedItem);
        }

        private async void GetRecommendations()
        {
            if (!AccountService.UserProfile.HasLogin) return;

            DailyRecommendPlaylists.Clear();
            DailyRecommendSongs.Clear();

            var playlists = await NetworkService.GetDailyRecommendPlaylistsAsync();
            var songs = await NetworkService.GetDailyRecommendSongsAsync();

            playlists?.ForEach(playlist => DailyRecommendPlaylists.Add(playlist));
            songs?.GetRange(0, 5).ForEach(song => DailyRecommendSongs.Add(song));
        }
    }
}