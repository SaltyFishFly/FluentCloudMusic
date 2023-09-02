using FluentCloudMusic.Controls;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.DataModels.ViewModels;
using FluentCloudMusic.Services;
using FluentCloudMusic.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FluentCloudMusic.Pages
{
    public sealed partial class AlbumPage : Page
    {
        private readonly ObservableCollection<Song> Songs;
        private readonly AlbumPageViewModel Album;

        public AlbumPage()
        {
            Songs = new ObservableCollection<Song>();
            Album = new AlbumPageViewModel();

            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Album.Source = e.Parameter as Album;

            var (isSuccess, album, songs) = await AlbumService.GetAlbumDetailAsync(Album.Id);
            if (!isSuccess) return;

            Album.Source = album;
            foreach (var song in songs) { Songs.Add(song); }
        }

        private void PlayAllButton_Click(object sender, RoutedEventArgs e)
        {
            var playlist = new List<ISong>(Songs);
            _ = MainPage.Player.PlayAsync(playlist);
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            var shareLink = $"https://music.163.com/#/album?id={Album.Id}";
            ClipboardUtil.SetText(shareLink);
            new Toast() { Content = ResourceUtil.Get("/Messages/CopiedToClipboardMessage") }.ShowAsync();
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
