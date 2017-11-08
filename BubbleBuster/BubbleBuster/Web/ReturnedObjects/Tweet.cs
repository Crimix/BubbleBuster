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
        [JsonProperty("full_text")]
        public string Text { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("entities")]
        public Entities Entities { get; set; }

        public Dictionary<string, int> NewsHyperlinks = new Dictionary<string, int>();

        public int hashtagBias = 0;
        public int positiveValue = 0;
        public int negativeValue = 0;
        public double mediaBias = 0;

        public List<String> posList = new List<string>();
        public List<String> negList = new List<string>();
        public List<String> tagList = new List<string>();

        //
        public int analysisConclusion = 0;

        public int getSentiment()
        {
            return positiveValue - negativeValue;
        }

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
