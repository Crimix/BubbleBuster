using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web.ReturnedObjects
{
    public class Friends
    {
        [JsonProperty("users")]
        public List<User> Users { get; set; }

        [JsonProperty("next_cursor")]
        public long NextCursor { get; set; }

    }
}
