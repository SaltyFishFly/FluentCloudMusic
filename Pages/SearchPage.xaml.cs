using FluentNetease.Classes;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentNetease.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public static SearchPage INSTANCE;

        private ObservableCollection<Song> ContentCollection;
        private SearchRequest CurrentSearchRequest;

        public SearchPage()
        {
            this.InitializeComponent();
            ContentCollection = new ObservableCollection<Song>();
            INSTANCE = this;
        }

        public async void Search(SearchRequest request)
        {
            CurrentSearchRequest = request;
            var (isSuccess, currentPage, searchResults) = await Network.SearchAsync(request);
            if (isSuccess)
            {
                ContentCollection.Clear();
                foreach (var item in searchResults)
                {
                    ContentCollection.Add(item);
                }
                PageText.Text = request.Section.Page.ToString() + " / " + currentPage.ToString();
                PreviousPageButton.IsEnabled = 1 < request.Section.Page;
                NextPageButton.IsEnabled = request.Section.Page < currentPage;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Search((SearchRequest)e.Parameter);
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            Search(CurrentSearchRequest.PrevPage());
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            Search(CurrentSearchRequest.NextPage());
        }
    }
}
