using Windows.ApplicationModel.Resources;

namespace FluentCloudMusic.Utils
{
    public static class ResourceUtil
    {
        public static string Get(string path)
        {
            return ResourceLoader.GetForCurrentView().GetString(path);
        }
    }
}
