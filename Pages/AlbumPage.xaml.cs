using FluentCloudMusic.DataModels;
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
        private readonly ObservableCollection<DeprecatedSong> Songs;
        private readonly AlbumViewModel Album;

        public AlbumPage()
        {
            Songs = new ObservableCollection<DeprecatedSong>();
            Album = new AlbumViewModel();

            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var (isSuccess, album) = await AlbumService.GetAlbumDetailAsync(((DeprecatedAlbum)e.Parameter).ID);
            if (!isSuccess) return;

            Album.Source = album;
        }
    }
}
