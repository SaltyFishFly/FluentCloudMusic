using Windows.Storage;

namespace FluentCloudMusic.Services
{
    public static class StorageService
    {
        private static readonly ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;

        public static T GetSetting<T>(string key)
        {
            return (T)Settings.Values[key];
        }

        public static object SetSetting(string key, object value)
        {
            Settings.Values[key] = value;
            return value;
        }

        public static bool RemoveSetting(string key)
        {
            return Settings.Values.Remove(key);
        }

        public static bool HasSetting(string key)
        {
            return Settings.Values.ContainsKey(key);
        }
    }
}