using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web.ReturnedObjects
{
    public class Entities
    {
        [JsonProperty("hashtags")]
        public List<Hashtag> HashTags { get; set; }

        [JsonProperty("urls")]
        public List<Url> Urls { get; set; }

    }
}
