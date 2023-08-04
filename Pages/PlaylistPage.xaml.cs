using FluentNetease.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
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
        public Playlist Playlist { get; set; }
        public PlaylistPage()
        {
            this.InitializeComponent();
            Songs = new ObservableCollection<Song>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Playlist = (Playlist)e.Parameter;
            var (isSuccess, jsonPlaylist, result) = await Network.GetPlaylistDetailAsync(Playlist.ID);
            if (isSuccess)
            {
                Playlist.CoverPictureUrl = jsonPlaylist["coverImgUrl"].ToString();
                Playlist.Name = jsonPlaylist["name"].ToString();
                Playlist.Description = jsonPlaylist["description"].ToString();

                foreach (var Item in result) Songs.Add(Item);
            }
        }

        private void MusicNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.PLAYER.Play(new NeteaseMusic { ID = (string)((FrameworkElement)sender).DataContext });
        }

        private void ArtistNameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AlbumNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.FRAME.Navigate(typeof(AlbumPage), ((FrameworkElement)sender).DataContext);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var PlayList = new List<AbstractMusic>();
            foreach (var Item in Songs)
            {
                PlayList.Add(Item.Music);
            }
            MainPage.PLAYER.Play(PlayList);
        }
    }
}
