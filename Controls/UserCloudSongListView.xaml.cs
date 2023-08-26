using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Pages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentCloudMusic.Controls
{
    public sealed partial class UserCloudSongListView : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<UserCloudSong>), typeof(UserCloudSongListView), new PropertyMetadata(new ObservableCollection<UserCloudSong>()));
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(UIElement), typeof(UserCloudSongListView), new PropertyMetadata(null));
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register("Footer", typeof(UIElement), typeof(UserCloudSongListView), new PropertyMetadata(null));
        public static readonly DependencyProperty IsToolBarEnabledProperty =
            DependencyProperty.Register("IsToolBarEnabled", typeof(bool), typeof(UserCloudSongListView), new PropertyMetadata(true));

        public ObservableCollection<UserCloudSong> ItemsSource
        {
            get => (ObservableCollection<UserCloudSong>)GetValue(ItemsSourceProperty);
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
        public bool IsToolBarEnabled
        {
            get => (bool)GetValue(IsToolBarEnabledProperty);
            set => SetValue(IsToolBarEnabledProperty, value);
        }

        private List<UserCloudSong> OriginalSongs;

        public UserCloudSongListView()
        {
            InitializeComponent();
        }

        public void ApplyFilter(string filter)
        {
            OriginalSongs ??= new List<UserCloudSong>(ItemsSource);
            ItemsSource.Clear();
            OriginalSongs.ForEach(song => { if (song.RelateTo(filter)) ItemsSource.Add(song); });
        }

        private void MusicNameButton_Click(object sender, RoutedEventArgs e)
        {
            var playlist = new List<ISong>(ItemsSource);
            int index = ItemsSource.IndexOf((sender as FrameworkElement).Tag as UserCloudSong);
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
    }
}
