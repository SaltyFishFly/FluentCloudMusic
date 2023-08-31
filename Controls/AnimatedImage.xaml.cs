using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FluentCloudMusic.Controls
{
    public sealed partial class AnimatedImage : UserControl
    {
        public event EventHandler<RoutedEventArgs> ImageOpened;

        public static DependencyProperty SourceProperty
            = DependencyProperty.Register("Source", typeof(string), typeof(AnimatedImage), new PropertyMetadata(null));

        private bool FirstLoad;

        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public AnimatedImage()
        {
            FirstLoad = true;

            InitializeComponent();

            RegisterPropertyChangedCallback(SourceProperty, (sender, args) =>
            {
                if (Source == null || Source == string.Empty) return;
                ImagePresenter.UriSource = new Uri(Source);
            });
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            ImageOpened?.Invoke(sender, e);
            if (FirstLoad)
            {
                ImageLoadStoryboard.Begin();
                FirstLoad = false;
            }
        }
    }
}
