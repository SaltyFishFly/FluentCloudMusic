using FluentCloudMusic.Controls;
using FluentCloudMusic.Services;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentCloudMusic.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
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
