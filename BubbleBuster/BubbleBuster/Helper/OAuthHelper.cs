using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public class OAuthHelper
    {
        public void DoStuff(string consumerKey, string accessToken)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<string> sortedData = new List<string>();

            var timestamp = DateTime.Now.ToUniversalTime();
            data["oauth_consumer_key"] = Uri.EscapeDataString(consumerKey);
            data["oauth_signature_method"] = Uri.EscapeDataString("HMAC-SHA1");
            data["oauth_timestamp"] = Uri.EscapeDataString(Convert.ToString((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds));
            data["oauth_nonce"] = Uri.EscapeDataString("a");
            data["oauth_token"] = Uri.EscapeDataString(accessToken);
            data["oauth_version"] = Uri.EscapeDataString("1.0");
            

        }
        
        
            /*
        data["oauth_signature_method"] = "HMAC-SHA1";
        data["oauth_timestamp"] = timestamp.toString();
        data["oauth_nonce"] = "a"; // Required, but Twitter doesn't appear to use it
        data["oauth_token"] = accessToken;
        data["oauth_version"] = "1.0";*/
        private bool include_entities = true;
        private string oauth_consumer_key = "";
        private string oauth_nonce = "";
        private string oauth_signature_method = "HMAC-SHA1";
        private string oauth_timestamp = "";
        private string oauth_token = "";
        private string oauth_version = "1.0";
    }
}

