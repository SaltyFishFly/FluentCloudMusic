using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace FluentNetease.Classes
{
    public class Account
    {
        public static UserProfile Profile { get; set; } = new UserProfile();

        public static async Task<int> LoginWithLocalSettings()
        {
            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
            if (AppSettings.Values.ContainsKey("Account") && AppSettings.Values.ContainsKey("Password"))
            {
                string CountryCode = (string)AppSettings.Values["CountryCode"];
                string Account = (string)AppSettings.Values["Account"];
                string Password = (string)AppSettings.Values["Password"];
                return await LoginAsync(CountryCode, Account, Password);
            }
            return 0;
        }

        /// <summary>
        /// 传入账号密码进行登录,登录失败弹出错误对话框
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public static async Task<int> LoginAsync(string countryCode, string account, string password)
        {
            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
            var (Code, RequestResult) = await Network.LoginAsync(countryCode, account, password);
            if (Code == 200)
            {
                AppSettings.Values["CountryCode"] = countryCode;
                AppSettings.Values["Account"] = account;
                AppSettings.Values["Password"] = password;
                Profile.SetLoginData(RequestResult);
            }
            else
            {
                AppSettings.Values["CountryCode"] = null;
                AppSettings.Values["Account"] = null;
                AppSettings.Values["Password"] = null;
            }
            return Code;
        }

        public static async void LogoutAsync()
        {
            bool IsSuccess = await Network.LogoutAsync();
            if (IsSuccess)
            {
                ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
                AppSettings.Values["CountryCode"] = null;
                AppSettings.Values["Account"] = null;
                AppSettings.Values["Password"] = null;
                Profile.Logout();
            }
        }

        public class UserProfile
        {
            public delegate void LoginEventHandler();
            public delegate void LogoutEventHandler();

            public event LoginEventHandler LoginEvent;
            public event LogoutEventHandler LogoutEvent;

            public bool LoginFlag { get; private set; }
            public string UserID { get; private set; }
            public string Nickname { get; private set; }
            public string AvatarUrl { get; private set; }
            public bool HasVip { get; private set; }

            public UserProfile()
            {
                LoginFlag = false;
            }

            public void SetLoginData(JObject RequestResult)
            {
                LoginFlag = true;
                UserID = RequestResult["profile"]["userId"].ToString();
                Nickname = RequestResult["profile"]["nickname"].ToString();
                AvatarUrl = RequestResult["profile"]["avatarUrl"].ToString();
                HasVip = RequestResult["profile"]["vipType"].Value<int>() != 0;
                LoginEvent();
            }

            public void Logout()
            {
                LoginFlag = false;
                LogoutEvent();
            }
        }
    }
}
