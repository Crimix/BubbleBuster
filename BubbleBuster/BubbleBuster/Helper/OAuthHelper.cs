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
        //Instance variable
        private static OAuthHelper _instance;

        //Private constructor such that it is a singleton
        private OAuthHelper()
        {
        }

        /// <summary>
        /// Returns a static instance of the class, if one exist.
        /// Else et creates a new instance.
        /// </summary>
        public static OAuthHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OAuthHelper();
                }
                return _instance;
            }
        }

        /// <summary>
        /// The method to be used for the request either Post or Get.
        /// </summary>
        public enum RequestMethod { POST, GET }; 

        /// <summary>
        /// Builds the auth header bases on the supplied parameters.
        /// </summary>
        /// <param name="method">The Get/Post method</param>
        /// <param name="twitterName">The name of the twitter user</param>
        /// <param name="accessToken">The access token</param>
        /// <param name="tokenSecret">The token secret</param>
        /// <param name="baseUrl">The base url</param>
        /// <param name="extraParameters">The extra parameters for the request</param>
        /// <returns>A auth header</returns>
        public string BuildAuthHeader(RequestMethod method, string twitterName, string accessToken, string tokenSecret, string baseUrl, Dictionary<string, string> extraParameters = null)
        {
            HMACSHA1 hmac = new HMACSHA1();
            string timestamp = Convert.ToString((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            string signingKey = Uri.EscapeDataString(Constants.CONSUMER_SECRET) + "&" + Uri.EscapeDataString(tokenSecret);
            string requestType = Enum.GetName(typeof(RequestMethod), method);
            string nonce = requestType + twitterName + timestamp;
            string signature_method = "HMAC-SHA1";
            string oauth_version = "1.0";

            //Append nonce to make it 32 characters
            while (nonce.Count() < 33)
                nonce += "a";

            //Add parameters to dictionary
            if(extraParameters == null)
            {
                extraParameters = new Dictionary<string, string>();
            }
            Dictionary<string, string> parameters = extraParameters;
            parameters.Add("oauth_consumer_key", Constants.CONSUMER_KEY);
            parameters.Add("oauth_nonce", nonce);
            parameters.Add("oauth_signature_method", signature_method);
            parameters.Add("oauth_timestamp", timestamp);
            parameters.Add("oauth_token", accessToken);
            parameters.Add("oauth_version", oauth_version);

            //Assemble Parameter String
            List<string> signatureParameters = new List<string>();
            string parameterString = "";

            foreach(string key in parameters.Keys)
            {
                signatureParameters.Add(Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(parameters[key]) + "&");
            }
            signatureParameters.Sort();

            foreach(string param in signatureParameters)
            {
                parameterString += param;
            }

            parameterString = parameterString.Trim('&');


            //Assemble Signature Base String
            string baseString = requestType + "&";
            baseString += Uri.EscapeDataString(baseUrl) + "&";
            baseString += Uri.EscapeDataString(parameterString);

            //Create oauth_signature
            string oauth_signature = HMACSHA1(signingKey, baseString);

            //Assemble Header String
            string headerString = "OAuth " +
                                  Uri.EscapeDataString("oauth_consumer_key") + "=\"" + Uri.EscapeDataString(Constants.CONSUMER_KEY) + "\"" +
                                  ", " + Uri.EscapeDataString("oauth_nonce") + "=\"" + Uri.EscapeDataString(nonce) + "\"" +
                                  ", " + Uri.EscapeDataString("oauth_signature") + "=\"" + Uri.EscapeDataString(oauth_signature) + "\"" +
                                  ", " + Uri.EscapeDataString("oauth_signature_method") + "=\"" + Uri.EscapeDataString(signature_method) + "\"" +
                                  ", " + Uri.EscapeDataString("oauth_timestamp") + "=\"" + Uri.EscapeDataString(timestamp) + "\"" +
                                  ", " + Uri.EscapeDataString("oauth_token") + "=\"" + Uri.EscapeDataString(accessToken) + "\"" +
                                  ", " + Uri.EscapeDataString("oauth_version") + "=\"" + Uri.EscapeDataString(oauth_version) + "\"";

            //return headerString;
            return headerString;
        }

        /// <summary>
        /// HMACSHA1 hashes the data using the key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="data">The data</param>
        /// <returns></returns>
        public static string HMACSHA1(string key, string data)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);
            var hmacsha1 = new HMACSHA1(keyByte);
            byte[] messageBytes = encoding.GetBytes(data);
            byte[] hashMessage = hmacsha1.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashMessage);
        }
    }
}

