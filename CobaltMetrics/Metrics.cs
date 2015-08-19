using System;

using CobaltMetrics.DataTypes.Generic;
using CobaltMetrics.DataTypes;
using Newtonsoft.Json.Linq;

namespace CobaltMetrics
{
    public class Metrics
    {
        //Session Info
        private static string userKey;
        private static string sessionID = Guid.NewGuid().ToString("N");
        private static long timestamp;

        //Metrics State
        private static MetricState currentState = MetricState.STOPPED;

        /// <summary>
        /// Creates a new metrics session, and begins accepting data.
        /// </summary>
        /// <param name="filePath">The file path for writing the XML data.</param>
        public static string StartMetrics(string userKey)
        {
            if (currentState == MetricState.RUNNING)
            {
                //throw new InvalidOperationException("The metrics system is already running!");
                Console.WriteLine("CMetrics WARNING: Previous Session was still running. Stopping...");
                StopMetrics();
            }

            //Set our run state to running.
            currentState = MetricState.RUNNING;

            //Set up our auth key and file paths for API queries.
            Metrics.userKey = userKey;

            //Generate a new Session ID
            sessionID = Guid.NewGuid().ToString("N");

            //Initialize our session.
            PostSessionInfo(true);

            return sessionID;
        }

        /// <summary>
        /// Adds data to the current running metrics session. NOTE: This should only be called with your custom implementation of GenericData. Included types do this automatically.
        /// </summary>
        /// <param name="rawData">A data object implementing the GenericData interface.</param>
        public static bool AddData(IGenericData rawData)
        {
            if (currentState != MetricState.RUNNING)
            {
                return false;
            }

            PostData(rawData);
            return true;
        }

        /// <summary>
        /// This should be called after you have finished collecting metrics data.
        /// </summary>
        public static bool StopMetrics()
        {
            if (currentState == MetricState.RUNNING)
            {
                //Post closing session data
                PostSessionInfo(false);

                currentState = MetricState.STOPPED;
                return true;
            }

            return false;
        }

        private static void PostData(IGenericData rawData)
        {
            JObject postData = new JObject();

            JObject sessionInfo = new JObject();

            sessionInfo.Add("user_key", userKey);
            sessionInfo.Add("session_id", sessionID);

            postData.Add("session_info", sessionInfo);

            JObject data = new JObject();

            data.Add("key", rawData.GetDataKey());
            data.Add("timestamp", rawData.GetTimestamp());
            data.Add("type", rawData.GetDataType().ToString().ToLower());

            switch (rawData.GetDataType())
            {
                case DataType.SINGLE:
                {
                    data.Add("value", rawData.GetDBDataValue());
                    break;
                }
                case DataType.ARRAY:
                {
                    JArray values = new JArray();

                    foreach (string element in rawData.GetDBDataValues())
                    {
                        values.Add(element);
                    }

                    data.Add("values", values);

                    break;
                }
            }

            postData.Add("data", data);

            HttpUtils.PostRequest("/data", postData, null);
        }

        private static void PostSessionInfo(bool isStart)
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            timestamp = (long)t.TotalMilliseconds;

            JObject postData = new JObject();

            JObject sessionInfo = new JObject();

            sessionInfo.Add("user_key", userKey);
            sessionInfo.Add("session_id", sessionID);

            if (isStart)
            {
                sessionInfo.Add("start_time", timestamp.ToString());
            }
            else
            {
                sessionInfo.Add("end_time", timestamp.ToString());
            }

            postData.Add("session_info", sessionInfo);

            HttpUtils.PostRequest("/session", postData, null);
        }
    }
}