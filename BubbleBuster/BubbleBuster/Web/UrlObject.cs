using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web
{
    public class UrlObject
    {
        public string Url { get; set; }

        public string BaseUrl { get; set; }

        public Dictionary<string, string> Params { get; set; }

        public UrlObject()
        {
            Params = new Dictionary<string, string>();
        }
    }
}
