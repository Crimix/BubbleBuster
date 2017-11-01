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

        private List<string> _ImportantWords = new List<string>();
        public List<string> ImportantWords { get { return _ImportantWords; } set { _ImportantWords = value; } }

        private Dictionary<string, int> _NewsHyperlinks = new Dictionary<string, int>();
        public Dictionary<string, int> NewsHyperlinks { get { return _NewsHyperlinks; } set { _NewsHyperlinks = value; } }

        public int EmotionValue { get { return emotionValue; } set { emotionValue = value; } }
        private int emotionValue = 0;

        public double Bias { get { return bias; } set { bias = value; } }
        private double bias = 0;

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
