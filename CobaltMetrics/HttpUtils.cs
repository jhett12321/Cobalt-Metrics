using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;
using System.Threading;

namespace CobaltMetrics
{
    public class HttpRequestData
    {
        public delegate void RequestCallback(string code, JObject data);

        public string uri;
        public JObject data;
        public RequestCallback callback;
    };

    class HttpUtils
    {
        private const string baseURL = "http://api.blackfeatherproductions.com/cobaltMetrics";

        public static void PostRequest(string uri, JObject data, HttpRequestData.RequestCallback callback)
        {
            var requestData = new HttpRequestData();
            requestData.uri = uri;
            requestData.data = data;
            requestData.callback = callback;

            System.Threading.ThreadPool.QueueUserWorkItem(HttpUtils.DoPost, requestData);
        }

        public static void GetRequest(string uri, AsyncCallback callback)
        {
            var requestData = new HttpRequestData();
            requestData.uri = uri;
            requestData.data = null;

            System.Threading.ThreadPool.QueueUserWorkItem(HttpUtils.DoGet, requestData);
        }

        private static void DoPost(object requestData)
        {
            HttpRequestData reqData = (HttpRequestData)requestData;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseURL + reqData.uri);

            request.Timeout = 5000;

            string postString = reqData.data.ToString(Formatting.None);

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postString);

            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            request.Method = "POST";

            Stream requestStream = request.GetRequestStream();

            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: posting of data failed.");

                Console.WriteLine("Response Code: " + response.StatusCode);
                Console.WriteLine("Response Description: " + response.StatusDescription);

                Console.WriteLine("Original JSON Request: ");
                Console.WriteLine(reqData.data.ToString(Formatting.Indented));
            }
        }

        private static void DoGet(object requestData)
        {
            HttpRequestData reqData = (HttpRequestData)requestData;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseURL + reqData.uri);

            request.Timeout = 5000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: retrieval of data failed.");
                Console.WriteLine("Response Code: " + response.StatusCode);
                Console.WriteLine("Response Description: " + response.StatusDescription);
                Console.WriteLine("Original GET Request: ");
                Console.WriteLine(baseURL + reqData.uri);
            }

            Stream responseStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(responseStream);

            string rawData = reader.ReadToEnd();

            JObject data = JObject.Parse(rawData);
            
            reqData.callback(response.StatusCode.ToString(), data);
        }
    }
}
