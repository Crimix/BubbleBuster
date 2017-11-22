using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web.ReturnedObjects.RateLimit
{
    public class ResourceUsers
    {
        [JsonProperty("/users/show/:id")]
        public Rate Status { get; set; }
    }
}
