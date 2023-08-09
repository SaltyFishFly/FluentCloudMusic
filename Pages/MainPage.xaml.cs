using FluentCloudMusic.Classes;
using FluentCloudMusic.Controls;
using FluentCloudMusic.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace FluentCloudMusic
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MusicPlayer PLAYER;
        public static Frame FRAME;

        public ObservableCollection<muxc.NavigationViewItem> CreatedPlaylistButtons { get; }
        public ObservableCollection<muxc.NavigationViewItem> BookmarkedPlaylistButtons { get; }

        private readonly Dictionary<string, Type> NavButtons = new Dictionary<string, Type>
        {
            { "NavItemDiscover", typeof(DiscoverPage) },
            { "NavItemHistory", typeof(HistoryPage) },
            { "NavItemDownloads", typeof(DownloadsPage) },
            { "NavItemCloud", typeof(CloudPage) },
            { "NavItemFavorites", typeof(FavoritesPage) },
            { "NavItemPodcasts", typeof(PodcastsPage) },
            { "NavItemAccount", typeof(AccountPage) },
            { "NavItemSettings", typeof(SettingsPage) }
        };

        public MainPage()
        {
            this.CreatedPlaylistButtons = new ObservableCollection<muxc.NavigationViewItem>();
            this.BookmarkedPlaylistButtons = new ObservableCollection<muxc.NavigationViewItem>();
            this.GeneratePlaylistButtons();
            this.InitializeComponent();
            PLAYER = MusicPlayer;
            FRAME = ContentFrame;
            this.MainNav.SelectedItem = NavItemDiscover;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            PLAYER.Dispose();
        }

        /// <summary>
        /// 获取用户的歌单并添加到左侧导航栏内
        /// </summary>
        private async void GeneratePlaylistButtons()
        {
            var (isSuccess, playlists) = await Network.GetUserPlaylist(Account.User.UserID);
            if (isSuccess)
            {
                CreatedPlaylistButtons.Clear();
                BookmarkedPlaylistButtons.Clear();

                foreach (var playlist in playlists)
                {
                    var item = new muxc.NavigationViewItem
                    {
                        Name = "NavItemPlaylist",
                        Tag = playlist,
                        Content = playlist.Name
                    };

                    if (playlist.CreatorID == Account.User.UserID)
                        CreatedPlaylistButtons.Add(item);
                    else
                        BookmarkedPlaylistButtons.Add(item);
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
            if (args.SelectedItemContainer == null) return;

            if (args.IsSettingsSelected)
                ContentFrame.Navigate(typeof(SettingsPage), args.SelectedItemContainer.Tag, args.RecommendedNavigationTransitionInfo);
            else if (args.SelectedItemContainer.Name == "NavItemPlaylist")
                ContentFrame.Navigate(typeof(PlaylistPage), args.SelectedItemContainer.Tag, args.RecommendedNavigationTransitionInfo);
            else
                ContentFrame.Navigate(NavButtons[args.SelectedItemContainer.Name], args.SelectedItemContainer.Tag, args.RecommendedNavigationTransitionInfo);
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
            // SourcePageType should not be null
            if (e.SourcePageType == null) return;
            MainNav.SelectedItem = FindNavigationItem(e.SourcePageType, e.Parameter);
            HeaderText.Text = ResourceLoader.GetForCurrentView().GetString($"Header{e.SourcePageType.Name}");
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

        private object FindNavigationItem(Type pageType, object navigationParameter)
        {
            if (NavButtons.ContainsValue(pageType))
            {
                // Account and settings button is stored in FooterItems
                // So can't be find regularly
                if (pageType == typeof(AccountPage)) return NavItemAccount;
                if (pageType == typeof(SettingsPage)) return MainNav.SettingsItem;

                return MainNav.MenuItems.OfType<muxc.NavigationViewItem>().First(n => NavButtons[n.Name] == pageType);
            }
            else
            {
                // Find from dynamic generated buttons
                if (pageType != typeof(PlaylistPage)) return null;
                return CreatedPlaylistButtons.Concat(BookmarkedPlaylistButtons)
                        .FirstOrDefault(button => ((Playlist)button.Tag).ID == ((Playlist)navigationParameter).ID);
            }
        }
    }
}