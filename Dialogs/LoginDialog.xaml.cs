using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentNetease.Dialogs
{
    public sealed partial class LoginDialog : ContentDialog
    {
        public LoginDialog()
        {
            this.InitializeComponent();
        }

        public async Task<(ContentDialogResult, string, string)> ShowDialogAsync()
        {
            ContentDialogResult Result = await ShowAsync();
            string account = AccountInputBox.Text;
            string password = PasswordInputBox.Password;
            return (Result, account, password);
        }

        private void AccountInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsPrimaryButtonEnabled = AccountInputBox.Text != "" && PasswordInputBox.Password != "" ? true : false;
        }

        private void PasswordInputBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            IsPrimaryButtonEnabled = AccountInputBox.Text != "" && PasswordInputBox.Password != "" ? true : false;
        }
    }
}
