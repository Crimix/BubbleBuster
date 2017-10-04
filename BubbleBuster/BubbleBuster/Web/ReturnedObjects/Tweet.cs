using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web.ReturnedObjects
{
    public class Tweet
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("entities")]
        public Entities Entities { get; set; }

        public List<string> ImportantWords { get; set; }

        public Dictionary<string, int> NewsHyperlinks { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Tweet)
            {
                return ((Tweet)obj).Id == Id;
            }
            else
            {
                return false;
            }
            
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
