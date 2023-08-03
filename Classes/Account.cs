using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace FluentNetease.Classes
{
    public static class Account
    {
        public static UserProfile User { get; set; } = new UserProfile();

        public delegate void LoginEventHandler(JObject loginResult);
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

        public class UserProfile
        {
            public bool LoginStatus { get; private set; }
            public string UserID { get; private set; }
            public string Nickname { get; private set; }
            public string AvatarUrl { get; private set; }
            public bool HasVip { get; private set; }

            public UserProfile()
            {
                LoginStatus = false;
                LoginEvent += OnLogin;
                LogoutEvent += OnLogout;
            }

            private void OnLogin(JObject loginResult)
            {
                LoginStatus = true;
                UserID = loginResult["profile"]["userId"].ToString();
                Nickname = loginResult["profile"]["nickname"].ToString();
                AvatarUrl = loginResult["profile"]["avatarUrl"].ToString();
                HasVip = loginResult["profile"]["vipType"].Value<int>() != 0;
            }

            public void OnLogout()
            {
                LoginStatus = false;
            }
        }
    }
}
