using FluentNetease.Classes;
using FluentNetease.Controls;
using FluentNetease.Pages;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
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
        public static MusicPlayer PLAYER_INSTANCE;
        public static Frame NAV_FRAME;

        private readonly Hashtable NavPages = new Hashtable
        {
            { "Discover", typeof(DiscoverPage) },
            { "History", typeof(HistoryPage) },
            { "Downloads", typeof(DownloadsPage) },
            {  "Cloud", typeof(CloudPage) },
            { "Favorites", typeof(FavoritesPage) },
            { "Podcasts", typeof(PodcastsPage) },
            { "Account", typeof(AccountPage) },
            { "Settings", typeof(SettingsPage) }
        };

        public MainPage()
        {
            InitializeComponent();
            MainNav.SelectedItem = NavItemDiscover;
            PLAYER_INSTANCE = MusicPlayer;
            NAV_FRAME = ContentFrame;
            NavItemAccount.Content = Account.Profile.Nickname;
            NavItemAccount.Icon = new ImageIcon()
            {
                Source = new BitmapImage(new Uri(Account.Profile.AvatarUrl))
            };
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
                {
                    MainNav_Navigate("Settings", args.RecommendedNavigationTransitionInfo);
                }
                else
                {
                    //导航
                    string ItemTag = args.SelectedItemContainer.Tag.ToString();
                    MainNav_Navigate(ItemTag, args.RecommendedNavigationTransitionInfo);
                }
            }
        }

        private void MainNav_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
        {
            Type Page = NavPages[navItemTag] as Type;
            if (Page != null && ContentFrame.CurrentSourcePageType != Page)
            {
                ContentFrame.Navigate(Page, null, transitionInfo);
            }
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
                //如果MainNav包含所选页面则更新
                foreach (DictionaryEntry Entry in NavPages)
                {
                    NavPageType = (Type)Entry.Value == e.SourcePageType ? e.SourcePageType : NavPageType;
                }
                if (NavPageType != null)
                {
                    //对两个特殊值做处理
                    if (NavPageType == typeof(AccountPage)) { SelectedItem = NavItemAccount; }
                    else if (NavPageType == typeof(SettingsPage)) { SelectedItem = (muxc.NavigationViewItem)MainNav.SettingsItem; }
                    //在Pages中查找相关页面
                    else { SelectedItem = MainNav.MenuItems.OfType<muxc.NavigationViewItem>().First(n => (Type)NavPages[n.Tag.ToString()] == e.SourcePageType); }
                }
                //否则置空
                MainNav.SelectedItem = SelectedItem;
                MainNav.Header = new TextBlock
                {
                    Margin = new Thickness { Left = -20 },
                    Text = ResourceLoader.GetForCurrentView().GetString(e.SourcePageType.Name + "Header"),
                    FontWeight = FontWeights.Normal
                };
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