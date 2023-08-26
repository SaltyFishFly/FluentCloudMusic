using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentCloudMusic.Controls
{
    public sealed partial class ToolBar : UserControl
    {
        public delegate void ButtonClickEventHandler(object sender, RoutedEventArgs e);
        public delegate void AutoSuggestBoxQuerySubmittedEventHandler(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e);

        public event ButtonClickEventHandler PlayAllButtonClickedEvent;
        public event ButtonClickEventHandler ShareButtonClickedEvent;
        public event ButtonClickEventHandler DownloadButtonClickedEvent;
        public event AutoSuggestBoxQuerySubmittedEventHandler FilterInputBoxQuerySubmittedEvent;

        public static readonly DependencyProperty IsPlayAllButtonEnabledProperty =
            DependencyProperty.Register("IsPlayAllButtonEnabled", typeof(bool), typeof(SongListView), new PropertyMetadata(true));
        public static readonly DependencyProperty IsShareButtonEnabledProperty =
            DependencyProperty.Register("IsShareButtonEnabled", typeof(bool), typeof(SongListView), new PropertyMetadata(true));
        public static readonly DependencyProperty IsDownloadButtonEnabledProperty =
            DependencyProperty.Register("IsDownloadButtonEnabled", typeof(bool), typeof(SongListView), new PropertyMetadata(false));
        public static readonly DependencyProperty IsFilterInputBoxEnabledProperty =
            DependencyProperty.Register("IsFilterInputBoxEnabled", typeof(bool), typeof(SongListView), new PropertyMetadata(true));

        public bool IsPlayAllButtonEnabled
        {
            get => (bool)GetValue(IsPlayAllButtonEnabledProperty);
            set => SetValue(IsPlayAllButtonEnabledProperty, value);
        }
        public bool IsShareButtonEnabled
        {
            get => (bool)GetValue(IsShareButtonEnabledProperty);
            set => SetValue(IsShareButtonEnabledProperty, value);
        }
        public bool IsDownloadButtonEnabled
        {
            get => (bool)GetValue(IsDownloadButtonEnabledProperty);
            set => SetValue(IsDownloadButtonEnabledProperty, value);
        }
        public bool IsFilterInputBoxEnabled
        {
            get => (bool)GetValue(IsFilterInputBoxEnabledProperty);
            set => SetValue(IsFilterInputBoxEnabledProperty, value);
        }

        public ToolBar()
        {
            InitializeComponent();
        }

        private void PlayAllButton_Click(object sender, RoutedEventArgs args) => 
            PlayAllButtonClickedEvent?.Invoke(sender, args);

        private void ShareButton_Click(object sender, RoutedEventArgs args) => 
            ShareButtonClickedEvent?.Invoke(sender, args);

        private void DownloadButton_Click(object sender, RoutedEventArgs args) => 
            DownloadButtonClickedEvent?.Invoke(sender, args);

        private void FilterInputBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) => 
            FilterInputBoxQuerySubmittedEvent?.Invoke(sender, args);
    }
}
