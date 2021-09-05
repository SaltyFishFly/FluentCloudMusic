using FluentNetease.Classes;
using FluentNetease.Dialogs;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentNetease.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void AccountInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInputBoxes();
        }

        private void PasswordInputBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckInputBoxes();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int Code = await Account.LoginAsync("86", AccountInputBox.Text, PasswordInputBox.Password);
            }
            catch
            {
                _ = new LoginFailedDialog().ShowAsync();
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckInputBoxes()
        {
            bool AccountIsEmpty = AccountInputBox.Text == string.Empty;
            bool PasswordIsEmpty = PasswordInputBox.Password == string.Empty;
            if (!AccountIsEmpty && !PasswordIsEmpty)
            {
                LoginButton.IsEnabled = true;
            }
            else
            {
                LoginButton.IsEnabled = false;
                PasswordInputBox.IsEnabled = !AccountIsEmpty;
                if (AccountIsEmpty)
                {
                    PasswordInputBox.Password = string.Empty;
                }
            }
        }
    }
}
