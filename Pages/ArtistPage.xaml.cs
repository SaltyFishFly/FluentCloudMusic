using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.ViewModels;
using FluentCloudMusic.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FluentCloudMusic.Pages
{
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
