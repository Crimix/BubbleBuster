using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web.ReturnedObjects.RateLimit
{
    public class Limit
    {
        [JsonProperty("resources")]
        public Resources Resources { get; set; }
    }
}
