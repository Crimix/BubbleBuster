using Newtonsoft.Json;

namespace BubbleBuster.Web.ReturnedObjects.RateLimit
{
    /// <summary>
    /// Class that can contain JSON information from Twitter. Deserilized using Newtonsoft.Json
    /// </summary>
    public class ResourceApplication
    {
        [JsonProperty("/application/rate_limit_status")]
        public Rate Status { get; set; }
    }
}
