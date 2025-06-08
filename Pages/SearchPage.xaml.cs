using FluentCloudMusic.Classes;
using FluentCloudMusic.DataModels;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Services;
using FluentCloudMusic.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FluentCloudMusic.Pages
{
    public sealed partial class SearchPage : Page
    {
        public static SearchPage Instance { get; private set; }

        private SearchRequest CurrentRequest
        {
            get => _CurrentRequest;
            set
            {
                _CurrentRequest = value;
                CurrentRequestViewModel.Source = value;
            }
        }

        private readonly ObservableCollection<Song> Songs;
        private readonly SearchPageViewModel CurrentRequestViewModel;
        private SearchRequest _CurrentRequest;

        public SearchPage()
        {
            Instance = this;

            Songs = new ObservableCollection<Song>();
            CurrentRequestViewModel = new SearchPageViewModel();

            InitializeComponent();
        }

        public async void Search(SearchRequest request)
        {
            try
            {
                CurrentRequest = request;

                var (pageCount, songs) = await NetworkService.SearchAsync(request);
                CurrentRequestViewModel.MaxPage = pageCount;

                Songs.Clear();
                foreach (var item in songs) Songs.Add(item);
            }
            catch (ResponseCodeErrorException) { }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Search((SearchRequest)e.Parameter);
        }

        private void PlayAllButtonClickedEvent(object sender, RoutedEventArgs e)
        {
            var playlist = new List<ISong>(Songs);
            _ = App.Player.PlayAsync(playlist);
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            Search(CurrentRequest.Prev());
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            Search(CurrentRequest.Next());
        }
    }
}
