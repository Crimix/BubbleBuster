using System.Collections.Generic;

namespace BubbleBuster.Web
{
    /// <summary>
    /// Used to contain information for the url for a Twitter request 
    /// </summary>
    public class RequestUrlObject
    {
        /// <summary>
        /// The full url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The base resource url
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// The parameters for the request
        /// </summary>
        public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();

    }
}
