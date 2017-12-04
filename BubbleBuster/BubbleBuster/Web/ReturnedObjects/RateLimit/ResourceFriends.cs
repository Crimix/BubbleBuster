using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
