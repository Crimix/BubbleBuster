using Newtonsoft.Json;

namespace BubbleBuster.Web.ReturnedObjects.RateLimit
{
    /// <summary>
    /// Class that can contain JSON information from Twitter. Deserilized using Newtonsoft.Json
    /// </summary>
    public class Limit
    {
        [JsonProperty("resources")]
        public Resources Resources { get; set; }
    }
}
