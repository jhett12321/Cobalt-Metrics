﻿using System.Collections.Generic;
using System;
using System.Xml.Linq;

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
        private static long startTime;

        //Session Data
        private static List<IGenericData> metricData;

        //Metrics State
        private static bool running = false;
        private static bool locked = false;

        //Metrics Setup
        private static string filePath;

        /// <summary>
        /// Creates a new metrics session, and begins accepting data.
        /// </summary>
        /// <param name="filePath">The file path for writing the XML data.</param>
        public static void StartMetrics(string userKey, String filePath)
        {
            if (running)
            {
                throw new InvalidOperationException("The metrics system is already running!");
            }

            if (locked)
            {
                throw new InvalidOperationException("The metrics system is currently locked, and is writing data. Consider a delay between metric sessions.");
            }

            if (running || locked)
            {
                return;
            }

            Metrics.userKey = userKey;
            Metrics.filePath = filePath;

            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            startTime = (long)t.TotalMilliseconds;

            metricData = new List<IGenericData>();
            running = true;
        }

        /// <summary>
        /// Adds data to the current running metrics session. NOTE: This should only be called with your custom implementation of GenericData. Included types do this automatically.
        /// </summary>
        /// <param name="rawData">A data object implementing the GenericData interface.</param>
        public static void AddData(IGenericData rawData)
        {
            if(!running || locked)
            {
                throw new InvalidOperationException("There is no current metric session active/available.");
            }

            else if(running && !locked)
            {
                metricData.Add(rawData);
                PostData(rawData);
            }
        }

        /// <summary>
        /// This should be called after you have finished collecting metrics data.
        /// </summary>
        public static void StopMetrics()
        {
            running = false;
            locked = true;

            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            long endTime = (long)t.TotalMilliseconds;

            XDocument doc;

            try
            {
                doc = XDocument.Load(filePath);
            }
            catch (Exception e)
            {
                doc = new XDocument(new XElement("sessions"));
            }

            if(doc.Element("sessions") == null)
            {
                doc = new XDocument(new XElement("sessions"));
            }

            XElement sessions = doc.Element("sessions");

            XElement session = new XElement("session");

            session.SetAttributeValue("sessionID", sessionID);
            session.SetAttributeValue("startTime", startTime.ToString());
            session.SetAttributeValue("endTime", endTime.ToString());
            session.SetAttributeValue("sessionTime", (endTime - startTime).ToString());

            foreach(IGenericData data in metricData)
            {
                if (data.GetDataKey() != null && data.GetTimestamp() > startTime && (data.GetDBDataValue() != null || data.GetDBDataValues() != null))
                {
                    XElement dataElement = new XElement("data");
                    dataElement.SetAttributeValue("key", data.GetDataKey());
                    dataElement.SetAttributeValue("timestamp", data.GetTimestamp());
                    dataElement.SetAttributeValue("type", data.GetDataType().ToString());

                    switch(data.GetDataType())
                    {
                        case DataType.SINGLE:
                        {
                            XElement valueElement = new XElement("value", data.GetDBDataValue());
                            dataElement.Add(valueElement);

                            break;
                        }
                        case DataType.ARRAY:
                        {
                            for (int i = 0; i < data.GetDBDataValues().Count; ++i )
                            {
                                XElement valueElement = new XElement("value" + i.ToString(), data.GetDBDataValues()[i]);
                                dataElement.Add(valueElement);
                            }

                            break;
                        }
                    }

                    session.Add(dataElement);
                }
            }

            sessions.Add(session);
            doc.Save(filePath, SaveOptions.None);
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
            HttpUtils.PostRequest("/data", postData);
        }
    }
}