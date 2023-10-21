using FluentCloudMusic.DataModels;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentCloudMusic.Services
{
    public static class AccountService
    {
        public static readonly User UserProfile = new User();

        public delegate void LoginEventHandler(Account profile);
        public delegate void LogoutEventHandler();

        public static event LoginEventHandler Login;
        public static event LogoutEventHandler Logout;

        public static async Task LoginAsync(string countryCode, string account, string password)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "countrycode", countryCode},
                { "phone", account },
                { "password", password }
            };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.LoginCellphone, parameters);
            var result = jsonResult.ToObject<LoginCellphoneResponse>();
            result.CheckCode(() => StorageService.RemoveSetting("LoginCookie"));

            StorageService.SetSetting("LoginCookie", App.API.Cookies.GetString());

            UserProfile.Account = result.Profile;
            Login(result.Profile);
        }

        public static async Task LoginByCookieAsync()
        {
            if (!StorageService.HasSetting("LoginCookie")) return;

            string loginCookie = StorageService.GetSetting<string>("LoginCookie");
            App.API.Cookies.LoadFromString(loginCookie);

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.LoginStatus);
            var result = jsonResult.ToObject<LoginStatusResponse>();
            result.CheckCode(() => StorageService.RemoveSetting("LoginCookie"));

            StorageService.SetSetting("LoginCookie", App.API.Cookies.GetString());

            UserProfile.Account = result.Profile;
            Login(result.Profile);
        }

        public static async Task LogoutAsync()
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Logout);
            var result = jsonResult.ToObject<BaseResponse>();
            result.CheckCode();

            StorageService.RemoveSetting("LoginCookie");

            UserProfile.Account = null;
            Logout();
        }
    }
}
