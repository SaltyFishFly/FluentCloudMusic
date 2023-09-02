using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.DataModels.ViewModels;
using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentCloudMusic.Services
{
    public static class AccountService
    {
        public static readonly UserProfile UserProfile = new UserProfile();

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
            var result = jsonResult.ToObject<LoginCellphoneResponse>(JsonUtil.Serializer);

            if (result.Code != 200)
            {
                StorageService.RemoveSetting("LoginCookie");
                return result.Code;
            }
            StorageService.SetSetting("LoginCookie", App.API.Cookies.GetString());

            UserProfile.Source = result.Profile;
            Login(result.Profile);

            return result.Code;
        }

        public static async Task<int> CheckLoginStatusAsync()
        {
            if (!StorageService.HasSetting("LoginCookie")) return -1;

            string loginCookie = StorageService.GetSetting<string>("LoginCookie");
            App.API.Cookies.LoadFromString(loginCookie);

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.LoginStatus);
            var result = jsonResult.ToObject<LoginStatusResponse>(JsonUtil.Serializer);

            if (result.Code != 200)
            {
                StorageService.RemoveSetting("LoginCookie");
                return result.Code;
            }
            StorageService.SetSetting("LoginCookie", App.API.Cookies.GetString());

            UserProfile.Source = result.Profile;
            Login(result.Profile);

            return result.Code;
        }

        public static async Task<bool> LogoutAsync()
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.Logout);
            var code = jsonResult["code"].Value<int>();

            if (code != 200) return false;
            
            StorageService.RemoveSetting("LoginCookie");

            UserProfile.Source = null;
            Logout();

            return true;
        }
    }
}
