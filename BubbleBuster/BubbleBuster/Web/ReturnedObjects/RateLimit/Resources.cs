﻿using Newtonsoft.Json;

namespace BubbleBuster.Web.ReturnedObjects.RateLimit
{
    /// <summary>
    /// Class that can contain JSON information from Twitter. Deserilized using Newtonsoft.Json
    /// </summary>
    public class Resources
    {
        [JsonProperty("statuses")]
        public ResourceStatuses Statuses { get; set; }

        [JsonProperty("friends")]
        public ResourceFriends Friends { get; set; }

        [JsonProperty("application")]
        public ResourceApplication Application { get; set; }

        [JsonProperty("users")]
        public ResourceUsers Users { get; set; }
    }
}
