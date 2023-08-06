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
    public sealed partial class AlbumPage : Page
    {
        private readonly ObservableCollection<Song> Songs;
        private readonly Album Album;

        public AlbumPage()
        {
            Songs = new ObservableCollection<Song>();
            Album = new Album();
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var album = e.Parameter as Album;
            var (isSuccess, albumInfo, songs) = await Network.GetAlbumDetailAsync(album.ID);
            if (isSuccess)
            {
                Album.ID = album.ID;
                Album.Name = albumInfo["name"].ToString();
                Album.Description = albumInfo["description"].ToString();
                Album.CoverPictureUrl = albumInfo["blurPicUrl"].ToString();

                foreach (var song in songs) Songs.Add(song);
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var playlist = new List<AbstractMusic>();
            foreach (var song in Songs) playlist.Add(song.Music);
            MainPage.PLAYER.Play(playlist);
        }

        private void MusicNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.PLAYER.Play((AbstractMusic)((FrameworkElement)sender).Tag);
        }
    }
}
