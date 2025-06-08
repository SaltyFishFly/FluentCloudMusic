using FluentCloudMusic.Classes;
using FluentCloudMusic.Controls;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Services;
using FluentCloudMusic.Utils;
using FluentCloudMusic.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace FluentCloudMusic.Pages
{
    public sealed partial class PlaylistPage : Page
    {
        private readonly ObservableCollection<Song> Songs;
        private readonly PlaylistPageViewModel Playlist;

        public PlaylistPage()
        {
            Songs = new ObservableCollection<Song>();
            Playlist = new PlaylistPageViewModel();

            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Playlist.Source = e.Parameter as Playlist;

            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("DailyRecommendPlaylistsToPlaylistPageAnimation");
            anim?.TryStart(SongListHeader.CoverImage);

            try
            {
                var (playlistInfo, songs) = await PlaylistService.GetPlaylistDetailAsync(Playlist.Id);
                Playlist.Source = playlistInfo;
                foreach (var song in songs) Songs.Add(song);
            }
            catch (ResponseCodeErrorException) { }
        }

        private void PlayAllButton_Click(object sender, RoutedEventArgs e)
        {
            var playlist = new List<ISong>(Songs);
            _ = App.Player.PlayAsync(playlist);
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            var shareLink = $"https://music.163.com/#/playlist?id={Playlist.Id}";
            ClipboardUtil.SetText(shareLink);
            new Toast()
            {
                Content = ResourceUtil.Get("/Messages/CopiedToClipboardMessage")
            }.ShowAsync();
        }

        private void DownloadButtonClickedEvent(object sender, RoutedEventArgs e)
        {

        }

        private void FilterInputBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            SongList.ApplyFilter(sender.Text);
        }
    }
}
