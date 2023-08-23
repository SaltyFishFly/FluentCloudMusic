using FluentCloudMusic.DataModels.JSONModels;
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
        public static readonly UserProfileViewModel UserProfile = new UserProfileViewModel();

        public delegate void LoginEventHandler(Profile profile);
        public delegate void LogoutEventHandler();

        public static event LoginEventHandler Login;
        public static event LogoutEventHandler Logout;

        public static async Task<int> LoginAsync(string countryCode, string account, string password)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "countrycode", countryCode},
                { "phone", account },
                { "password", password }
            };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.LoginCellphone, parameters);
            var result = jsonResult.ToObject<LoginCellphoneResponse>();

            if (result.Code != 200)
            {
                StorageService.RemoveSetting("LoginCookie");
                return result.Code;
            }
            StorageService.SetSetting("LoginCookie", App.API.Cookies.GetString());

            UserProfile.Profile = result.Profile;
            Login(result.Profile);

            return result.Code;
        }

        public static async Task<int> CheckLoginStatusAsync()
        {
            if (!StorageService.HasSetting("LoginCookie")) return -1;

            string loginCookie = StorageService.GetSetting<string>("LoginCookie");
            App.API.Cookies.LoadFromString(loginCookie);

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.LoginStatus);
            var result = jsonResult.ToObject<LoginStatusResponse>();

            if (result.Code != 200)
            {
                StorageService.RemoveSetting("LoginCookie");
                return result.Code;
            }
            StorageService.SetSetting("LoginCookie", App.API.Cookies.GetString());

            UserProfile.Profile = result.Profile;
            Login(result.Profile);

            return result.Code;
        }

        public static async Task<bool> LogoutAsync()
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Logout);
            var code = jsonResult["code"].Value<int>();

            if (code != 200) return false;
            
            StorageService.RemoveSetting("LoginCookie");

            UserProfile.Profile = null;
            Logout();

            return true;
        }
    }

    public class UserProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Profile Profile
        {
            set
            {
                HasLogin = value != null;
                _UserProfile = value;
                Notify(nameof(HasLogin));
                Notify(nameof(UserID));
                Notify(nameof(Nickname));
                Notify(nameof(AvatarUrl));
                Notify(nameof(HasVip));
            }
        }
        public bool HasLogin { get; private set; }
        public string UserID => HasLogin ? _UserProfile.UserID : string.Empty;
        public string Nickname => HasLogin ? _UserProfile.Nickname : string.Empty;
        public string AvatarUrl => HasLogin ? _UserProfile.AvatarUrl : "ms-appx:///Assets/LargeTile.scale-400.png";
        public bool HasVip => HasLogin && _UserProfile.VipType != 0;

        private Profile _UserProfile;

        public UserProfileViewModel()
        {
            HasLogin = false;
            _UserProfile = null;
        }

        private void Notify(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
    }
}
