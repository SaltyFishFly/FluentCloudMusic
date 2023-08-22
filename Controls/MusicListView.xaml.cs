using FluentCloudMusic.DataModels;
using FluentCloudMusic.Pages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentCloudMusic.Controls
{
    public sealed partial class MusicListView : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<Song>), typeof(MusicListView), new PropertyMetadata(new ObservableCollection<Song>()));
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(UIElement), typeof(MusicListView), new PropertyMetadata(null));
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register("Footer", typeof(UIElement), typeof(MusicListView), new PropertyMetadata(null));
        public static readonly DependencyProperty IsArtistButtonEnabledProperty =
            DependencyProperty.Register("IsArtistButtonEnabled", typeof(bool), typeof(MusicListView), new PropertyMetadata(true));
        public static readonly DependencyProperty IsAlbumButtonEnabledProperty =
            DependencyProperty.Register("IsAlbumButtonEnabled", typeof(bool), typeof(MusicListView), new PropertyMetadata(true));
        public static readonly DependencyProperty IsToolBarEnabledProperty =
            DependencyProperty.Register("IsToolBarEnabled", typeof(bool), typeof(MusicListView), new PropertyMetadata(true));

        public ObservableCollection<Song> ItemsSource
        {
            get { return (ObservableCollection<Song>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public UIElement Header
        {
            get { return (UIElement)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public UIElement Footer
        {
            get { return (UIElement)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }
        public bool IsArtistButtonEnabled
        {
            get { return (bool)GetValue(IsArtistButtonEnabledProperty); }
            set { SetValue(IsArtistButtonEnabledProperty, value); }
        }
        public bool IsAlbumButtonEnabled
        {
            get { return (bool)GetValue(IsAlbumButtonEnabledProperty); }
            set { SetValue(IsAlbumButtonEnabledProperty, value); }
        }
        public bool IsToolBarEnabled
        {
            get { return (bool)GetValue(IsToolBarEnabledProperty); }
            set { SetValue(IsToolBarEnabledProperty, value); }
        }

        private List<Song> OriginalSongs;

        public MusicListView()
        {
            InitializeComponent();
        }

        private void ApplyFilter(string filter)
        {
            OriginalSongs ??= new List<Song>(ItemsSource);
            ItemsSource.Clear();
            OriginalSongs.ForEach(song => { if (song.RelateTo(filter)) ItemsSource.Add(song); });
        }

        private void MusicNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Player.Play((Song)((FrameworkElement)sender).Tag);
        }

        private void ArtistNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Navigate(typeof(ArtistPage), ((FrameworkElement)sender).Tag);
        }

        private void AlbumNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Navigate(typeof(AlbumPage), ((FrameworkElement)sender).Tag);
        }

        private void PlayAllButton_Click(object sender, RoutedEventArgs e)
        {
            var playlist = new List<Song>(ItemsSource);
            MainPage.Player.Play(playlist);
        }

        private void FilterInputBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ApplyFilter(sender.Text);
        }

        private void MenuFlyout_Opened(object sender, object e)
        {
            var subMenu = (sender as MenuFlyout).Items.FirstOrDefault(i => i.Name == "SongFlyoutMenuArtistsButton") as MenuFlyoutSubItem;
            if (subMenu == null) return;

            subMenu.Items.Clear();
            foreach (var artist in (Artists)subMenu.Tag)
            {
                var artistButton = new MenuFlyoutItem()
                {
                    Tag = artist,
                    Text = artist.Name
                };
                artistButton.Click += ArtistNameButton_Click;
                subMenu.Items.Add(artistButton);
            }
        }
    }
}
