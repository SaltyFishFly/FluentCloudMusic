using FluentCloudMusic.Controls;
using FluentCloudMusic.Services;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FluentCloudMusic.Pages
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountService.UserProfile.HasLogin) return;

            try
            {
                await AccountService.LoginAsync("86", AccountInputBox.Text, PasswordInputBox.Password);
            }
            catch (Exception ex)
            {
                _ = new CommonDialog("登陆错误", ex.Message, "知道了").ShowAsync();
            }
        }
    }
}
