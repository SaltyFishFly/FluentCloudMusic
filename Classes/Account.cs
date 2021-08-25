using FluentNetease.Dialogs;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace FluentNetease.Classes
{
    public class Account
    {
        public static Account INSTANCE = new Account();

        public delegate void UserProfileChangedHandler(UserProfile profile);
        public event UserProfileChangedHandler ProfileChanged;
        public UserProfile Profile { get; set; }
        public async Task<int> LoginWithLocalSettings()
        {
            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
            if (AppSettings.Values.ContainsKey("Account") && AppSettings.Values.ContainsKey("Password"))
            {
                string Account = (string)AppSettings.Values["Account"];
                string Password = (string)AppSettings.Values["Password"];
                return await LoginAsync(Account, Password);
            }
            return 0;
        }

        public async void LoginWithDialogAsync()
        {
            (ContentDialogResult, string, string) DialogResult = await new LoginDialog() { IsPrimaryButtonEnabled = false }.ShowDialogAsync();
            if (DialogResult.Item1 == ContentDialogResult.Primary)
            {
                await LoginAsync(DialogResult.Item2, DialogResult.Item3);
            }
        }

        /// <summary>
        /// 传入账号密码进行登录,登录失败弹出错误对话框
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        public async Task<int> LoginAsync(string Account, string Password)
        {
            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
            var Result = await Network.LoginAsync(Account, Password);
            if (Result.Item1 == 200)
            {
                AppSettings.Values["Account"] = Account;
                AppSettings.Values["Password"] = Password;
                Profile = new UserProfile(Result.Item2);
                ProfileChanged(Profile);
            }
            else
            {
                AppSettings.Values["Account"] = null;
                AppSettings.Values["Password"] = null;
                _ = new LoginFailedDialog().SetErrorCode(Result.Item1).ShowAsync();
            }
            return Result.Item2["code"].Value<int>();
        }

        public class UserProfile
        {
            public string Nickname { get; set; }
            public string AvatarUrl { get; set; }

            public UserProfile(JObject RequestResult)
            {
                Nickname = RequestResult["profile"]["nickname"].ToString();
                AvatarUrl = RequestResult["profile"]["avatarUrl"].ToString();
            }
        }
    }
}
