using FluentCloudMusic.DataModels;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.ViewModels;
using FluentCloudMusic.Services;
using System.Collections.ObjectModel;
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
        private readonly AlbumViewModel Album;

        public AlbumPage()
        {
            Songs = new ObservableCollection<Song>();
            Album = new AlbumViewModel();

            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var (isSuccess, album, songs) = await AlbumService.GetAlbumDetailAsync(((Album)e.Parameter).Id);
            if (!isSuccess) return;

            Album.Source = album;
            foreach (var song in songs) { Songs.Add(song); }
        }
    }
}
