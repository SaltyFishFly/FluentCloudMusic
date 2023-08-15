using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace FluentCloudMusic.Classes
{
    public static class Account
    {
        public static readonly UserProfile User = new UserProfile();

        public delegate void LoginEventHandler(JObject loginInfo);
        public delegate void LogoutEventHandler();

        public static event LoginEventHandler LoginEvent;
        public static event LogoutEventHandler LogoutEvent;

        /// <summary>
        /// 传入账号密码进行登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public static async Task<int> LoginAsync(string countryCode, string account, string password)
        {
            var (code, jsonResult) = await Network.LoginAsync(countryCode, account, password);
            if (code != 200)
            {
                Storage.RemoveSetting("LoginCookie");
                return code;
            }
            Storage.SetSetting("LoginCookie", App.API.Cookies.GetString());

            LoginEvent(jsonResult);
            return code;
        }

        public static async Task<int> CheckLoginStatus()
        {
            if (!Storage.HasSetting("LoginCookie")) return -1;

            string loginCookie = Storage.GetSetting<string>("LoginCookie");
            App.API.Cookies.LoadFromString(loginCookie);

            var (code, jsonResult) = await Network.CheckLoginStatus();
            if (code != 200)
            {
                Storage.RemoveSetting("LoginCookie");
                return code;
            }

            LoginEvent(jsonResult);
            return code;
        }

        public static async void LogoutAsync()
        {
            bool success = await Network.LogoutAsync();
            if (success)
            {
                Storage.RemoveSetting("LoginCookie");
                LogoutEvent();
            }
        }

        public class UserProfile : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public bool HasLogin { get; private set; }
            public string UserID { get; private set; }
            public string Nickname { get; private set; }
            public string AvatarUrl { get; private set; }
            public bool HasVip { get; private set; }

            public UserProfile()
            {
                HasLogin = false;
                UserID = string.Empty;
                Nickname = string.Empty;
                AvatarUrl = "ms-appx:///Assets/LargeTile.scale-400.png";
                HasVip = false;

                LoginEvent += OnLogin;
                LogoutEvent += OnLogout;
            }

            private void OnLogin(JObject loginInfo)
            {
                HasLogin = true;
                Notify(nameof(HasLogin));

                UserID = loginInfo["profile"]["userId"].ToString();
                Notify(nameof(UserID));

                Nickname = loginInfo["profile"]["nickname"].ToString();
                Notify(nameof(Nickname));

                AvatarUrl = loginInfo["profile"]["avatarUrl"].ToString();
                Notify(nameof(AvatarUrl));

                HasVip = loginInfo["profile"]["vipType"].Value<int>() != 0;
                Notify(nameof(HasVip));
            }

            private void OnLogout()
            {
                HasLogin = false;
                Notify(nameof(HasLogin));

                UserID = string.Empty;
                Notify(nameof(UserID));

                Nickname = string.Empty;
                Notify(nameof(Nickname));

                AvatarUrl = "ms-appx:///Assets/LargeTile.scale-400.png";
                Notify(nameof(AvatarUrl));

                HasVip = false;
                Notify(nameof(HasVip));
            }

            private void Notify(string member)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
            }
        }
    }
}
