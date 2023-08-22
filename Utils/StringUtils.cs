namespace FluentCloudMusic.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// 比较两个图片字符串是否指向不同服务器上的相同图片，如
        /// URL1 = https://p1.music.126.net/114514.jpg?1919
        /// URL2 = https://p2.music.126.net/114514.jpg?810
        /// URL1与URL2实际指向相同的资源，此函数返回true
        /// </summary>
        /// <param name="url1WithoutQuery">URL字符串1</param>
        /// <param name="url2WithOutQuery">URL字符串2</param>
        /// <returns>URL1和URL2是否相同</returns>
        public static bool IsSameImageUrl(string url1, string url2)
        {
            string url1WithoutQuery = RemoveQuery(url1);
            string url2WithoutQuery = RemoveQuery(url2);

            // Find the indices of the first three '/' characters in the URLs
            int thirdSlashIndex1 = url1WithoutQuery.IndexOf('/', url1WithoutQuery.IndexOf('/', url1WithoutQuery.IndexOf('/') + 1) + 1);
            int thirdSlashIndex2 = url2WithoutQuery.IndexOf('/', url2WithoutQuery.IndexOf('/', url2WithoutQuery.IndexOf('/') + 1) + 1);

            // URLs don't have at least three '/' characters
            if (thirdSlashIndex1 < 0 || thirdSlashIndex2 < 0) return false;

            // Get the substrings after the third '/' character
            string segment1 = url1WithoutQuery.Substring(thirdSlashIndex1 + 1);
            string segment2 = url2WithoutQuery.Substring(thirdSlashIndex2 + 1);

            // Compare the segments
            return segment1 == segment2;
        }

        public static string RemoveQuery(string url)
        {
            int queryIndex = url.IndexOf('?');
            return queryIndex >= 0 ? url.Substring(0, queryIndex) : url;
        }
    }
}
