using FluentCloudMusic.Services;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentCloudMusic.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
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
            App.SetTheme((ElementTheme)index);
        }
    }
}
