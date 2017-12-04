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
    public class Friends
    {

        [JsonProperty("users")]
        public List<User> Users { get; set; } = new List<User>();

        [JsonProperty("next_cursor")]
        public long NextCursor { get; set; }
    }
}
