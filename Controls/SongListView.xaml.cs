using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Pages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentCloudMusic.Controls
{
    public sealed partial class SongListView : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<Song>), typeof(SongListView), new PropertyMetadata(new ObservableCollection<Song>()));
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(UIElement), typeof(SongListView), new PropertyMetadata(null));
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register("Footer", typeof(UIElement), typeof(SongListView), new PropertyMetadata(null));
        public static readonly DependencyProperty IsArtistButtonEnabledProperty =
            DependencyProperty.Register("IsArtistButtonEnabled", typeof(bool), typeof(SongListView), new PropertyMetadata(true));
        public static readonly DependencyProperty IsAlbumButtonEnabledProperty =
            DependencyProperty.Register("IsAlbumButtonEnabled", typeof(bool), typeof(SongListView), new PropertyMetadata(true));

        public ObservableCollection<Song> ItemsSource
        {
            get => (ObservableCollection<Song>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public UIElement Header
        {
            get => (UIElement)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        public UIElement Footer
        {
            get => (UIElement)GetValue(FooterProperty);
            set => SetValue(FooterProperty, value);
        }
        public bool IsArtistButtonEnabled
        {
            get => (bool)GetValue(IsArtistButtonEnabledProperty);
            set => SetValue(IsArtistButtonEnabledProperty, value);
        }
        public bool IsAlbumButtonEnabled
        {
            get => (bool)GetValue(IsAlbumButtonEnabledProperty);
            set => SetValue(IsAlbumButtonEnabledProperty, value);
        }

        private List<Song> OriginalSongs;

        public SongListView()
        {
            InitializeComponent();
        }

        public void ApplyFilter(string filter)
        {
            OriginalSongs ??= new List<Song>(ItemsSource);
            ItemsSource.Clear();
            OriginalSongs.ForEach(song => { if (song.RelateTo(filter)) ItemsSource.Add(song); });
        }

        private void MusicNameButton_Click(object sender, RoutedEventArgs e)
        {
            var playlist = new List<ISong>(ItemsSource);
            int index = ItemsSource.IndexOf((sender as FrameworkElement).Tag as Song);
            _ = MainPage.Player.PlayAsync(playlist, index);
        }

        private void ArtistNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Navigate(typeof(ArtistPage), ((FrameworkElement)sender).Tag);
        }

        private void AlbumNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Navigate(typeof(AlbumPage), ((FrameworkElement)sender).Tag);
        }

        private void MenuFlyout_Opened(object sender, object e)
        {
            var subMenu = (sender as MenuFlyout).Items.FirstOrDefault(i => i.Name == "SongFlyoutMenuArtistsButton") as MenuFlyoutSubItem;
            if (subMenu == null) return;

            subMenu.Items.Clear();
            foreach (var artist in (Artist[])subMenu.Tag)
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
