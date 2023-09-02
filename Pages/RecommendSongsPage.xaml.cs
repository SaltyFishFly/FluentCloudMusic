using FluentCloudMusic.DataModels.ViewModels;
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
    }
}
