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
        public enum DataType { POST, GET }; //Expand based on what data is needed

        public string BuildAuthHeader(DataType type, string twitterName, string consumerKey, string consumerSecret, string accessToken, string tokenSecret)
        {
            HMACSHA1 hmac = new HMACSHA1();
            string timestamp = Convert.ToString((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            //string timestamp = "1318622958";
            string requestType = Enum.GetName(typeof(DataType), type);
            string signingKey = Uri.EscapeDataString(consumerSecret) + "&" + Uri.EscapeDataString(tokenSecret);
            string nonce = requestType + twitterName + timestamp;
            //string nonce = "kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg";
            string signature_method = "HMAC-SHA1";
            string oauth_version = "1.0";
            string signature = "";
            
            string baseURL = "https://api.twitter.com//1.1/statuses/update.json?include_entities=true";

            string temp_signature = "include_entities=true" +
                               "&" + Uri.EscapeDataString("oauth_consumer_key") + "=" + Uri.EscapeDataString(consumerKey) +
                               "&" + Uri.EscapeDataString("oauth_nonce") + "=" + Uri.EscapeDataString(nonce) +
                               "&" + Uri.EscapeDataString("oauth_signature_method") + "=" + Uri.EscapeDataString(signature_method) +
                               "&" + Uri.EscapeDataString("oauth_timestamp") + "=" + Uri.EscapeDataString(timestamp) +
                               "&" + Uri.EscapeDataString("oauth_token") + "=" + Uri.EscapeDataString(accessToken) +
                               "&" + Uri.EscapeDataString("oauth_version") + "=" + Uri.EscapeDataString(oauth_version);

            signature = requestType + 
                        "&" + Uri.EscapeDataString(baseURL) +
                        "&" + Uri.EscapeDataString(temp_signature);

            string headerString = "OAuth " +
                                  Uri.EscapeDataString("oauth_consumer_key") + "=\"" + Uri.EscapeDataString(consumerKey)+ "\"" +
                                  ", " + Uri.EscapeDataString("oauth_nonce") + "=\"" + Uri.EscapeDataString(nonce) + "\"" +
                                  ", " + Uri.EscapeDataString("oauth_signature") + "=\"" + Uri.EscapeDataString(HMACSHA1(signingKey, signature)) + "\"" +
                                  ", " + Uri.EscapeDataString("oauth_signature_method") + "=\"" + Uri.EscapeDataString(signature_method) + "\"" +
                                  ", " + Uri.EscapeDataString("oauth_timestamp") + "=\"" + Uri.EscapeDataString(timestamp) + "\"" +
                                  ", " + Uri.EscapeDataString("oauth_token") + "=\"" + Uri.EscapeDataString(accessToken) + "\"" +
                                  ", " + Uri.EscapeDataString("oauth_version") + "=\"" + Uri.EscapeDataString(oauth_version) + "\"";

            return headerString;
            
        }

        //Partly borrowed from https://salesforce.stackexchange.com/questions/92589/why-doesnt-hmacsha1-generate-the-same-hash-as-my-c-code
        public static string HMACSHA1(string key, string data)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);
            var hmacsha1 = new HMACSHA1(keyByte);
            byte[] messageBytes = encoding.GetBytes(data);
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashmessage);
        }
    }
}

