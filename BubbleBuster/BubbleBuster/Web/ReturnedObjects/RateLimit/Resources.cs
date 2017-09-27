
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web.ReturnedObjects.RateLimit
{
    public class Resources
    {
        [JsonProperty("statuses")]
        public ResourceStatuses Statuses { get; set; }

        [JsonProperty("friends")]
        public ResourceFriends Friends { get; set; }

        [JsonProperty("application")]
        public ResourceApplication Application { get; set; }

    }
}
