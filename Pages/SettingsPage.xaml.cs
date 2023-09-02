using FluentCloudMusic.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FluentCloudMusic.Pages
{
    public sealed partial class SettingsPage : Page
    {
        public const string ThemeSetting = "Theme";

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ThemeButtons_Loaded(object sender, RoutedEventArgs e)
        {
            if (!StorageService.HasSetting(ThemeSetting))
                ThemeButtons.SelectedItem = AutoTheme;
            else
                ThemeButtons.SelectedIndex = StorageService.GetSetting<int>(ThemeSetting);
        }

        private void ThemeButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = ThemeButtons.SelectedIndex;
            if (index == -1) return;

            StorageService.SetSetting(ThemeSetting, index);
            App.UpdateTheme();
        }
    }
}
