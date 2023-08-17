using FluentCloudMusic.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentCloudMusic.Pages
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

            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var (isSuccess, detailedAlbum, songs) = await ((Album)e.Parameter).GetDetail();
            if (!isSuccess) return;
            detailedAlbum.CopyTo(Album);
            foreach (var song in songs) Songs.Add(song);
            MusicList.ApplyFilter(string.Empty);
        }
    }
}
