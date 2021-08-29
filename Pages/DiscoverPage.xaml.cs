using FluentNetease.Classes;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentNetease.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DiscoverPage : Page
    {
        public ObservableCollection<Playlist> ContentCollection;
        public DiscoverPage()
        {
            this.InitializeComponent();
            ContentCollection = new ObservableCollection<Playlist> { };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadPageContent();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainPage.FRAME.Navigate(typeof(PlaylistPage), (Playlist)e.ClickedItem);
        }

        private async void LoadPageContent()
        {
            ContentCollection.Clear();
            var RequestResult = await Network.GetDailyRecommendPlaylistAsync();
            if (RequestResult != null)
            {
                foreach (var Item in RequestResult)
                {
                    ContentCollection.Add(Item);
                }
            }
        }
    }
}