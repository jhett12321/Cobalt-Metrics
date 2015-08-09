using System;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace CobaltMetrics
{
    class HttpUtils
    {
        private const string baseURL = "http://api.blackfeatherproductions.com/cobaltMetrics";

        public static bool PostRequest(string uri, JObject data)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(baseURL + uri);

            string postString = data.ToString(Formatting.None);

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postString);

            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            request.Method = "POST";

            Stream requestStream = request.GetRequestStream();

            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: posting of data failed.");

                Console.WriteLine("Response Code: " + response.StatusCode);
                Console.WriteLine("Response Description: " + response.StatusDescription);

                Console.WriteLine("Original JSON Request: ");
                Console.WriteLine(data.ToString(Formatting.Indented));

                return false;
            }

            return true;
        }

        public static JObject GetRequest(string uri)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(baseURL + uri);

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: retrieval of data failed.");
                Console.WriteLine("Response Code: " + response.StatusCode);
                Console.WriteLine("Response Description: " + response.StatusDescription);
                Console.WriteLine("Original GET Request: ");
                Console.WriteLine(baseURL + uri);
                return null;
            }

            Stream responseStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(responseStream);

            string rawData = reader.ReadToEnd();

            JObject data = JObject.Parse(rawData);

            return data;
        }
    }
}
