using FluentNetease.Classes;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentNetease.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DiscoverPage : Page
    {
        public ObservableCollection<Playlist> PlaylistList;
        public DiscoverPage()
        {
            this.InitializeComponent();
            PlaylistList = new ObservableCollection<Playlist> { };
            UpdatePlayList();
        }

        private async void UpdatePlayList()
        {
            PlaylistList.Clear();
            var ResultList = await Network.GetDailyPlaylistAsync();
            foreach (var Result in ResultList)
            {
                PlaylistList.Add(Result);
            }
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainPage.NAV_FRAME.Navigate(typeof(PlaylistPage), (Playlist)e.ClickedItem);
        }
    }
}