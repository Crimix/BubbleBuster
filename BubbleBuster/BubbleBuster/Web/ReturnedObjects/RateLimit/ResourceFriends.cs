using Newtonsoft.Json;

namespace BubbleBuster.Web.ReturnedObjects.RateLimit
{
    /// <summary>
    /// Class that can contain JSON information from Twitter. Deserilized using Newtonsoft.Json
    /// </summary>
    public class ResourceFriends
    {
        [JsonProperty("/friends/list")]
        public Rate List { get; set; }

        [JsonProperty("/friends/ids")]
        public Rate Ids { get; set; }
    }
}
