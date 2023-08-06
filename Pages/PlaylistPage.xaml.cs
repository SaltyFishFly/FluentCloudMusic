using FluentNetease.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentNetease.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PlaylistPage : Page
    {
        private readonly ObservableCollection<Song> Songs;
        private readonly Playlist Playlist;

        public PlaylistPage()
        {
            Songs = new ObservableCollection<Song>();
            Playlist = new Playlist();
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var playlist = e.Parameter as Playlist;
            var (isSuccess, playlistInfo, songs) = await Network.GetPlaylistDetailAsync(playlist.ID);
            if (isSuccess)
            {
                Playlist.ID = playlist.ID;
                Playlist.Name = playlistInfo["name"].ToString();
                Playlist.Description = playlistInfo["description"].ToString();
                Playlist.CoverPictureUrl = playlistInfo["coverImgUrl"].ToString();

                foreach (var song in songs) Songs.Add(song);
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var playlist = new List<AbstractMusic>();
            foreach (var song in Songs) playlist.Add(song.Music);
            MainPage.PLAYER.Play(playlist);
        }
    }
}
