using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FluentCloudMusic.Pages
{
    public sealed partial class RecommendSongsPage : Page
    {
        private readonly RecommendSongsPageViewModel ViewModel;

        public RecommendSongsPage()
        {
            ViewModel = new RecommendSongsPageViewModel();

            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.LoadContents();
        }

        private async void PlayAllButtonClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var playlist = new List<ISong>(ViewModel.Songs);
            await App.Player.PlayAsync(playlist);
        }
    }
}
