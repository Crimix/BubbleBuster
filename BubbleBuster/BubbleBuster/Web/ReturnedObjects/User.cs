using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web.ReturnedObjects
{
    /// <summary>
    /// Class that can contain JSON information from Twitter. Deserilized using Newtonsoft.Json
    /// </summary>
    public class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("protected")]
        public bool IsProtected { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
