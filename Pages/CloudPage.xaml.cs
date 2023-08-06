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
    public sealed partial class CloudPage : Page
    {
        private ObservableCollection<Song> Songs;
        private SearchSection CurrentSearchSection;

        public CloudPage()
        {
            this.InitializeComponent();
            Songs = new ObservableCollection<Song>();
        }

        private async void GetUserCloud(SearchSection section)
        {
            CurrentSearchSection = section;
            var (isSuccess, currentPage, songList) = await Network.GetUserCloudAsync(section);
            if (isSuccess)
            {
                Songs.Clear();
                foreach (var Item in songList) Songs.Add(Item);

                PageText.Text = section.Page.ToString() + " / " + currentPage.ToString();
                PreviousPageButton.IsEnabled = 1 < section.Page;
                NextPageButton.IsEnabled = section.Page < currentPage;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetUserCloud(new SearchSection());
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
