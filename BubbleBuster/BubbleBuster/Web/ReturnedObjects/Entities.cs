using Newtonsoft.Json;
using System.Collections.Generic;

namespace BubbleBuster.Web.ReturnedObjects
{
    /// <summary>
    /// Class that can contain JSON information from Twitter. Deserilized using Newtonsoft.Json
    /// </summary>
    public class Entities
    {
        [JsonProperty("urls")]
        public List<Url> Urls { get; set; }
    }
}
