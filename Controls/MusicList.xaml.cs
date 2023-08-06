using FluentNetease.Classes;
using FluentNetease.Pages;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentNetease.Controls
{
    public sealed partial class MusicList : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(MusicList), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(MusicList), new PropertyMetadata(null));
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register("Footer", typeof(object), typeof(MusicList), new PropertyMetadata(null));
        public static readonly DependencyProperty ArtistButtonEnabledProperty =
            DependencyProperty.Register("ArtistButtonEnabled", typeof(bool), typeof(MusicList), new PropertyMetadata(null));
        public static readonly DependencyProperty AlbumButtonEnabledProperty =
            DependencyProperty.Register("AlbumButtonEnabled", typeof(bool), typeof(MusicList), new PropertyMetadata(null));

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public object Footer
        {
            get { return GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }
        public bool ArtistButtonEnabled
        {
            get { return (bool)GetValue(ArtistButtonEnabledProperty); }
            set { SetValue(ArtistButtonEnabledProperty, value); }
        }
        public bool AlbumButtonEnabled
        {
            get { return (bool)GetValue(AlbumButtonEnabledProperty); }
            set { SetValue(AlbumButtonEnabledProperty, value); }
        }

        public MusicList()
        {
            this.Header = null;
            this.ItemsSource = new ObservableCollection<Song>();
            this.ArtistButtonEnabled = true;
            this.AlbumButtonEnabled = true;
            this.InitializeComponent();
        }

        private void MusicNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.PLAYER.Play((AbstractMusic)((FrameworkElement)sender).Tag);
        }

        private void ArtistNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.FRAME.Navigate(typeof(ArtistPage), ((FrameworkElement)sender).Tag);
        }

        private void AlbumNameButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.FRAME.Navigate(typeof(AlbumPage), ((FrameworkElement)sender).Tag);
        }
    }
}
