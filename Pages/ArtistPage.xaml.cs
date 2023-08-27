using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.ViewModels;
using FluentCloudMusic.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentCloudMusic.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ArtistPage : Page
    {
        private ArtistViewModel Artist { get; }

        public ArtistPage()
        {
            Artist = new ArtistViewModel();

            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Artist.Source = e.Parameter as Artist;

            var (isSuccess, artist) = await ArtistService.GetArtistDetailAsync(Artist.Id);
            if (!isSuccess) return;

            Artist.Source = artist;
        }
    }
}
