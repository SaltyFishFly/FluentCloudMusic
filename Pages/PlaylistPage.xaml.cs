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
        private ObservableCollection<Song> ContentCollection;
        public PlaylistPage()
        {
            this.InitializeComponent();
            ContentCollection = new ObservableCollection<Song>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Playlist PlayList = (Playlist)e.Parameter;
            var (IsSuccess, PlaylistInfo, Result) = await Network.GetPlaylistDetailAsync(PlayList.ID);
            if (IsSuccess)
            {
                CoverPicture.ImageSource = new BitmapImage()
                {
                    UriSource = new Uri(PlaylistInfo["coverImgUrl"].ToString())
                };
                TitleText.Text = PlaylistInfo["name"].ToString();
                DescriptionText.Text += PlaylistInfo["description"].ToString();
                foreach (var Item in Result)
                {
                    ContentCollection.Add(Item);
                }
            }
        }

        private void MusicNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.PLAYER_INSTANCE.Play(new Music { ID = (string)((FrameworkElement)sender).DataContext });
        }

        private void ArtistNameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AlbumNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.NAV_FRAME.Navigate(typeof(AlbumPage), ((FrameworkElement)sender).DataContext);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var PlayList = new List<Music>();
            foreach (var Item in ContentCollection)
            {
                PlayList.Add(Item.Music);
            }
            MainPage.PLAYER_INSTANCE.Play(PlayList);
        }
    }
}
