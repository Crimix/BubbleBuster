using Newtonsoft.Json;

namespace BubbleBuster.Web.ReturnedObjects.RateLimit
{
    /// <summary>
    /// Class that can contain JSON information from Twitter. Deserilized using Newtonsoft.Json
    /// </summary>
    public class ResourceStatuses
    {
        [JsonProperty("/statuses/user_timeline")]
        public Rate UserTimeLine { get; set; }
    }
}
