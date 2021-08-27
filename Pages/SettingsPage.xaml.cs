using FluentNetease.Classes;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentNetease.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void LogoutButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Account.LogoutAsync();
        }
    }
}
