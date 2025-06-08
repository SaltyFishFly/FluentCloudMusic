using FluentCloudMusic.Classes;
using FluentCloudMusic.Controls;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Utils;
using FluentCloudMusic.ViewModels;
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
        private readonly AlbumPageViewModel ViewModel;

        public AlbumPage()
        {
            Songs = new ObservableCollection<Song>();
            ViewModel = new AlbumPageViewModel();

            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                var album = e.Parameter as Album;
                ViewModel.Source = album;

                var (detailedAlbum, songs) = await album.GetDetailAsync();
                ViewModel.Source = detailedAlbum;

                foreach (var song in songs) { Songs.Add(song); }
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
            var shareLink = $"https://music.163.com/#/album?id={ViewModel.Id}";
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
