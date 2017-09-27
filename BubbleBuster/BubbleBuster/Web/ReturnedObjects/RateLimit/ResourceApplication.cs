using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web.ReturnedObjects.RateLimit
{
    public class ResourceApplication
    {
        [JsonProperty("/application/rate_limit_status")]
        public Rate Status { get; set; }
    }
}
