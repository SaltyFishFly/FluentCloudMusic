using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.ViewModels;
using FluentCloudMusic.Services;
using FluentCloudMusic.Utils;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace FluentCloudMusic.Pages
{
    public sealed partial class DiscoverPage : Page
    {
        public readonly DiscoverPageViewModel ViewModel;

        public DiscoverPage()
        {
            ViewModel = new DiscoverPageViewModel();

            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (AccountService.UserProfile.HasLogin) ViewModel.LoadContents();
        }

        private void PlaylistItem_Click(object sender, ItemClickEventArgs e)
        {
            // Apply transition animation to PlaylistPage
            var imageContainer = 
                VisualTreeUtil.FindChildByName(DailyRecommendPlaylistsGridView.ContainerFromItem(e.ClickedItem), "CoverImageContainer");
            ConnectedAnimationService
                .GetForCurrentView()
                .PrepareToAnimate("DailyRecommendPlaylistsToPlaylistPageAnimation", (UIElement)imageContainer);

            MainPage.Navigate(typeof(PlaylistPage), (Playlist)e.ClickedItem);
        }

        private void DailyRecommendSongsViewAllButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Navigate(typeof(RecommendSongsPage), null);
        }
    }
}