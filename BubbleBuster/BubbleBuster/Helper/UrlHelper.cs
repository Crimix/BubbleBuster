namespace BubbleBuster.Helper
{
    public class UrlHelper
    {

        /// <summary>
        /// Shortens an url by removing the prefix and postfix. Such as http:// and .com
        /// </summary>
        /// <param name="url">The url string</param>
        /// <returns>A shorten url string</returns>
        public static string ShortenUrl(string url)
        {
            string[] words = { "www.", "http://", "https://", "www1." };
            string[] endWorkds = { ".com/", ".org/", ".co.uk/", ".net/", ".mit.edu/", ".ca/", ".org.il/", ".edu/", ".us/" };

            foreach (var item in words)
            {
                if (url.Contains(item))
                    url = url.Substring(url.IndexOf(item) + item.Length);
            }

            foreach (var item in endWorkds)
            {
                if (url.Contains(item) && !url.EndsWith(item))
                    url = url.Remove(url.IndexOf(item) + item.Length);
            }

            return url;
        }
    }
}
