using Newtonsoft.Json;

namespace BubbleBuster.Web.ReturnedObjects.RateLimit
{
    /// <summary>
    /// Class that can contain JSON information from Twitter. Deserilized using Newtonsoft.Json
    /// </summary>
    public class ResourceUsers
    {
        [JsonProperty("/users/show/:id")]
        public Rate Status { get; set; }
    }
}
