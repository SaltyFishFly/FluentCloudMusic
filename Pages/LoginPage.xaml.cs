using FluentCloudMusic.Classes;
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
            catch (ResponseCodeErrorException ex)
            {
                new Toast()
                { 
                    Content = $"登录错误(错误代码: {ex.Code})\n{ex.Message}"
                }.ShowAsync();
            }
            catch (Exception ex)
            {
                new Toast()
                {
                    Content = $"登录错误\n{ex.Message}"
                }.ShowAsync();
            }
        }
    }
}
