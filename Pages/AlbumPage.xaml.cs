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
    public sealed partial class AlbumPage : Page
    {
        public ObservableCollection<Song> ContentCollection;

        public AlbumPage()
        {
            this.InitializeComponent();
            ContentCollection = new ObservableCollection<Song>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var (IsSuccess, AlbumInfo, SongList) = await Network.GetAlbumDetailAsync((string)e.Parameter);
            if (IsSuccess)
            {
                CoverPicture.ImageSource = new BitmapImage()
                {
                    UriSource = new Uri(AlbumInfo["blurPicUrl"].ToString())
                };
                TitleText.Text = AlbumInfo["name"].ToString();
                DescriptionText.Text = AlbumInfo["description"].ToString();
                foreach (var Item in SongList)
                {
                    ContentCollection.Add(Item);
                }
            }
        }
        
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var PlayList = new List<Music>();
            foreach (var Item in ContentCollection)
            {
                PlayList.Add(Item.Music);
            }
            MainPage.PLAYER.Play(PlayList);
        }

        private void MusicNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.PLAYER.Play(new Music() { ID = (string)((FrameworkElement)sender).DataContext });
        }
    }
}
