using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.ViewModels;
using FluentCloudMusic.Services;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentCloudMusic.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PlaylistPage : Page
    {
        private readonly ObservableCollection<Song> Songs;
        private readonly PlaylistViewModel Playlist;

        public PlaylistPage()
        {
            Songs = new ObservableCollection<Song>();
            Playlist = new PlaylistViewModel();
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Playlist.Source = e.Parameter as Playlist;

            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("DailyRecommendPlaylistsToPlaylistPageAnimation");
            anim?.TryStart(MusicListHeader.CoverImage);

            var (isSuccess, playlistInfo, songs) = await NetworkService.GetPlaylistDetailAsync(Playlist.Id);
            if (isSuccess)
            {
                Playlist.Source = playlistInfo;
                foreach (var song in songs) Songs.Add(song);
            }
        }
    }
}
