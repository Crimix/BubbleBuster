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
    /// Also contains more information
    /// </summary>
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

        /// <summary>
        /// Contains the bias of the keyword
        /// </summary>
        public int KeywordBias { get; set; } = 0;

        /// <summary>
        /// Contains the positive value for the tweet
        /// </summary>
        public int PositiveValue { get; set; } = 0;

        /// <summary>
        /// Contains the negative value for the tweet 
        /// </summary>
        public int NegativeValue { get; set; } = 0;

        /// <summary>
        /// Contains the bias for media, the links in the tweet
        /// </summary>
        public double MediaBias { get; set; } = 0;

        /// <summary>
        /// Boolean for if a tweet contains quotes
        /// </summary>
        public bool HasQuotes { get; set; } = false;

        /// <summary>
        /// String array for the quotes
        /// </summary>
        public string[] Quotes { get; set; }

        /// <summary>
        /// Wordlist for the positive words
        /// </summary>
        public List<String> PosList { get; set; } = new List<string>();

        /// <summary>
        /// Wordlist for the negative words
        /// </summary>
        public List<String> NegList { get; set; } = new List<string>();

        /// <summary>
        /// Wordlist for the keywords
        /// </summary>
        public List<String> TagList { get; set; } = new List<string>();

        /// <summary>
        /// Returns the sentiment of the tweet
        /// </summary>
        /// <returns>The sentiment as an int</returns>
        public int GetSentiment()
        {
            return PositiveValue - NegativeValue;
        }

        //Overridden such that a tweet object can be compared based on id
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

        //Overridden because Equals is
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
