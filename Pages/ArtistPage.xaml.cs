using FluentCloudMusic.Classes;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FluentCloudMusic.Pages
{
    public sealed partial class ArtistPage : Page
    {
        private ArtistPageViewModel Artist { get; }

        public ArtistPage()
        {
            Artist = new ArtistPageViewModel();

            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                var artist = e.Parameter as Artist;
                Artist.Source = artist;

                var detailedArtist = await artist.GetDetailAsync();
                Artist.Source = detailedArtist;
            }
            catch (ResponseCodeErrorException) { }
        }
    }
}
