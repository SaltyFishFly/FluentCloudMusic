using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FluentCloudMusic.Services
{
    public static class AccountService
    {
        public static readonly UserProfile User = new UserProfile();

        public delegate void LoginEventHandler(JObject loginInfo);
        public delegate void LogoutEventHandler();

        public static event LoginEventHandler LoginEvent;
        public static event LogoutEventHandler LogoutEvent;

        public static async Task<int> LoginAsync(string countryCode, string account, string password)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "countrycode", countryCode},
                { "phone", account },
                { "password", password }
            };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.LoginCellphone, parameters);
            var code = jsonResult["code"].Value<int>();

            if (code != 200)
            {
                StorageService.RemoveSetting("LoginCookie");
                return code;
            }
            StorageService.SetSetting("LoginCookie", App.API.Cookies.GetString());

            LoginEvent(jsonResult);
            return code;
        }

        public static async Task<int> CheckLoginStatusAsync()
        {
            if (!StorageService.HasSetting("LoginCookie")) return -1;

            string loginCookie = StorageService.GetSetting<string>("LoginCookie");
            App.API.Cookies.LoadFromString(loginCookie);

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.LoginStatus);
            var code = jsonResult["code"].Value<int>();

            if (code != 200)
            {
                StorageService.RemoveSetting("LoginCookie");
                return code;
            }
            StorageService.SetSetting("LoginCookie", App.API.Cookies.GetString());

            LoginEvent(jsonResult);
            return code;
        }

        public static async Task<bool> LogoutAsync()
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Logout);
            var code = jsonResult["code"].Value<int>();

            if (code != 200) return false;
            
            StorageService.RemoveSetting("LoginCookie");
            LogoutEvent();
            return true;
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
