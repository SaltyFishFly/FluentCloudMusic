using FluentCloudMusic.Controls;
using FluentCloudMusic.DataModels;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.Pages;
using FluentCloudMusic.Services;
using System;
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
        private static readonly Dictionary<string, Type> NavButtons = new Dictionary<string, Type>
        {
            { "NavItemDiscover" , typeof(DiscoverPage)  },
            { "NavItemHistory"  , typeof(HistoryPage)   },
            { "NavItemDownloads", typeof(DownloadsPage) },
            { "NavItemCloud"    , typeof(CloudPage)     },
            { "NavItemFavorites", typeof(FavoritesPage) },
            { "NavItemPodcasts" , typeof(PodcastsPage)  },
            { "NavItemLogin"    , typeof(LoginPage)     },
            { "NavItemAccount"  , typeof(AccountPage)   },
            { "NavItemSettings" , typeof(SettingsPage)  },
        };

        public static MusicPlayerControl Player;

        private static new Frame Frame;

        public ObservableCollection<muxc.NavigationViewItem> CreatedPlaylistButtons { get; }

        public ObservableCollection<muxc.NavigationViewItem> BookmarkedPlaylistButtons { get; }

        public MainPage()
        {
            CreatedPlaylistButtons = new ObservableCollection<muxc.NavigationViewItem>();
            BookmarkedPlaylistButtons = new ObservableCollection<muxc.NavigationViewItem>();
            InitializeComponent();
            Player = MusicPlayer;
            Frame = ContentFrame;
            AccountService.Login += OnLogin;
            AccountService.Logout += OnLogout;
            MainNav.SelectedItem = NavItemDiscover;

            _ = AccountService.CheckLoginStatusAsync();
        }

        public static bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride = null)
        {
            if (Frame == null) return false;
            return Frame.Navigate(sourcePageType, parameter, infoOverride);
        }

        /// <summary>
        /// 获取用户的歌单并添加到左侧导航栏内
        /// </summary>
        private async void GeneratePlaylistButtons()
        {
            var (isSuccess, playlists) = await PlaylistService.GetUserPlaylist(AccountService.UserProfile.UserId);

            if (!isSuccess) return;

            CreatedPlaylistButtons.Clear();
            BookmarkedPlaylistButtons.Clear();

            playlists.ForEach(playlist =>
            {
                var item = new muxc.NavigationViewItem()
                {
                    Name = "NavItemPlaylist",
                    Tag = playlist,
                    Content = playlist.Name
                };

                if (playlist.Creator.UserId == AccountService.UserProfile.UserId) CreatedPlaylistButtons.Add(item);
                else BookmarkedPlaylistButtons.Add(item);
            });
        }

        private object FindNavigationItem(Type destPageType, object navigationParameter)
        {
            if (NavButtons.ContainsValue(destPageType))
            {
                return MainNav.MenuItems
                    .Concat(MainNav.FooterMenuItems)
                    .OfType<muxc.NavigationViewItem>()
                    .First(item =>
                    {
                        NavButtons.TryGetValue(item.Name, out Type itemPageType);
                        return itemPageType == destPageType;
                    });
            }
            else
            {
                // Find from dynamic generated buttons
                if (destPageType != typeof(PlaylistPage)) return null;

                return CreatedPlaylistButtons
                    .Concat(BookmarkedPlaylistButtons)
                    .FirstOrDefault(button => ((Playlist)button.Tag).Id == ((Playlist)navigationParameter).Id);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            AccountService.Login -= OnLogin;
            AccountService.Logout -= OnLogout;
            Player.Dispose();
        }

        private void OnLogin(Profile profile)
        {
            GeneratePlaylistButtons();
            ContentFrame.Navigate(typeof(DiscoverPage), null);
            ContentFrame.BackStack.Clear();
        }

        private void OnLogout()
        {
            ContentFrame.Navigate(typeof(LoginPage), null);
            ContentFrame.BackStack.Clear();
        }

        /// <summary>
        /// 负责处理导航事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MainNav_SelectionChanged(muxc.NavigationView sender, muxc.NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItemContainer;
            if (item == null) return;

            if (item.Name == "NavItemPlaylist")
                ContentFrame.Navigate(typeof(PlaylistPage), item.Tag, args.RecommendedNavigationTransitionInfo);
            else
                ContentFrame.Navigate(NavButtons[item.Name], item.Tag, args.RecommendedNavigationTransitionInfo);
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
            if (sender.Text == string.Empty) return;

            var request = new SearchRequest(sender.Text);
            if (ContentFrame.CurrentSourcePageType == typeof(SearchPage))
                (ContentFrame.Content as SearchPage).Search(request);
            else
                ContentFrame.Navigate(typeof(SearchPage), request);
        }
    }
}