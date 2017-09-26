using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web.ReturnedObjects.RateLimit
{
    public class ResourceStatuses
    {
        [JsonProperty("/statuses/user_timeline")]
        public Rate UserTimeLine { get; set; }
    }
}
