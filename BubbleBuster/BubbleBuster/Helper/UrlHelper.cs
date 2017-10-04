using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public class UrlHelper
    {
        private static UrlHelper _instance;

        private UrlHelper()
        {

        }

        public static UrlHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UrlHelper();
                }
                return _instance;
            }
        }

        public string ShortenUrl(string url)
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
                if (url.Contains(item))
                    url = url.Remove(url.IndexOf(item) + item.Length);
            }

            return url;

        }
    }
}
