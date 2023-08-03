using FluentNetease.Classes;
using FluentNetease.Controls;
using FluentNetease.Pages;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace FluentNetease
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MusicPlayer PLAYER;
        public static Frame FRAME;

        public ObservableCollection<muxc.NavigationViewItem> PlaylistsCreated { get; }
        public ObservableCollection<muxc.NavigationViewItem> PlaylistsBookmarked { get; }

        private readonly Hashtable NavPages = new Hashtable
        {
            { "Discover", typeof(DiscoverPage) },
            { "History", typeof(HistoryPage) },
            { "Downloads", typeof(DownloadsPage) },
            { "Cloud", typeof(CloudPage) },
            { "Favorites", typeof(FavoritesPage) },
            { "Podcasts", typeof(PodcastsPage) },
            { "Account", typeof(AccountPage) },
            { "Settings", typeof(SettingsPage) }
        };

        public MainPage()
        {
            InitializeComponent();
            PlaylistsCreated = new ObservableCollection<muxc.NavigationViewItem>();
            PlaylistsBookmarked = new ObservableCollection<muxc.NavigationViewItem>();
            MainNav.SelectedItem = NavItemDiscover;
            PLAYER = MusicPlayer;
            FRAME = ContentFrame;
            GetUserPlaylists();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            PLAYER.Dispose();
        }

        /// <summary>
        /// 获取用户的歌单并添加到左侧导航栏内
        /// </summary>
        private async void GetUserPlaylists()
        {
            var (IsSuccess, RequestResult) = await Network.GetUserPlaylist(Account.User.UserID);
            if (IsSuccess)
            {
                PlaylistsCreated.Clear();
                PlaylistsBookmarked.Clear();
                foreach (var Item in RequestResult)
                {
                    var PlaylistItem = new muxc.NavigationViewItem
                    {
                        Tag = "Playlist",
                        Content = Item.Name,
                        DataContext = Item
                    };
                    PlaylistItem.Tag = "Playlist";
                    if (Item.CreatorID == Account.User.UserID)
                        PlaylistsCreated.Add(PlaylistItem);
                    else
                        PlaylistsBookmarked.Add(PlaylistItem);
                }
            }
        }

        /// <summary>
        /// 负责处理导航事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MainNav_SelectionChanged(muxc.NavigationView sender, muxc.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                //对Settings按钮作特殊处理
                if (args.IsSettingsSelected)
                    MainNav_Navigate("Settings", args.RecommendedNavigationTransitionInfo);
                if (args.SelectedItemContainer.Tag.ToString() == "Playlist")
                    ContentFrame.Navigate(typeof(PlaylistPage), args.SelectedItemContainer.DataContext);
                else
                    //导航
                    MainNav_Navigate(args.SelectedItemContainer.Tag.ToString(), args.RecommendedNavigationTransitionInfo);
            }
        }

        private void MainNav_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
        {
            Type Page = NavPages[navItemTag] as Type;
            if (Page != null && ContentFrame.CurrentSourcePageType != Page)
                ContentFrame.Navigate(Page, null, transitionInfo);
        }

        private void MainNav_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
        {
            ContentFrame.GoBack();
        }

        /// <summary>
        /// 导航后更新MainNav.Header与MainNav.SelectedItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            //先过滤掉源为null
            if (e.SourcePageType != null)
            {
                Type NavPageType = null;
                muxc.NavigationViewItem SelectedItem = null;
                //先在预先设置好的按钮中找
                foreach (DictionaryEntry Entry in NavPages)
                {
                    if ((Type)Entry.Value == e.SourcePageType)
                    {
                        NavPageType = e.SourcePageType;
                        break;
                    }
                }
                if (NavPageType != null)
                {
                    //因为Account按钮在FooterItems里，所以在MenuItems里找不到，需要提前判断好
                    if (NavPageType == typeof(AccountPage)) { SelectedItem = NavItemAccount; }
                    else if (NavPageType == typeof(SettingsPage)) { SelectedItem = (muxc.NavigationViewItem)MainNav.SettingsItem; }
                    //在Pages中查找相关页面
                    else { SelectedItem = MainNav.MenuItems.OfType<muxc.NavigationViewItem>().First(n => (Type)NavPages[n.Tag.ToString()] == e.SourcePageType); }
                }
                //如果前面没找到就在动态生成的按钮里找
                else
                {
                    foreach (var Item in PlaylistsCreated)
                    {
                        if (Item.DataContext == e.Parameter)
                        {
                            SelectedItem = Item;
                            break;
                        }
                    }
                }
                MainNav.SelectedItem = SelectedItem;
                HeaderText.Text = ResourceLoader.GetForCurrentView().GetString(e.SourcePageType.Name + "Header");
            }
        }

        private void NavSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (sender.Text != string.Empty)
            {
                SearchRequest Request = new SearchRequest(sender.Text);
                if (ContentFrame.CurrentSourcePageType == typeof(SearchPage))
                {
                    SearchPage.INSTANCE.Search(Request);
                }
                else
                {
                    ContentFrame.Navigate(typeof(SearchPage), Request);
                }
            }
        }
    }
}