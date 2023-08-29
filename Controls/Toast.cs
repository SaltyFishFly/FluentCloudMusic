using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace FluentCloudMusic.Controls
{
    public class Toast : Control
    {
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(string), typeof(Toast), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(Toast), new PropertyMetadata(TimeSpan.FromSeconds(3.0)));

        public Toast()
        {
            DefaultStyleKey = typeof(Toast);

            Width = Window.Current.Bounds.Width;
            Height = Window.Current.Bounds.Height;
            Transitions = new TransitionCollection();

            Window.Current.SizeChanged += Current_SizeChanged;
        }

        public TimeSpan Duration
        {
            get => (TimeSpan)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public string Content
        {
            get => (string)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            Width = Window.Current.Bounds.Width;
            Height = Window.Current.Bounds.Height;
        }

        public async void ShowAsync()
        {
            Transitions.Add(new EntranceThemeTransition()); 
            var popup = new Popup
            {
                IsOpen = true,
                Child = this
            };

            await Task.Delay(Duration);

            Transitions.Clear();
            Transitions.Add(new PopupThemeTransition());
            popup.Child = null;
            popup.IsOpen = false;

            Window.Current.SizeChanged -= Current_SizeChanged;
        }
    }
}
