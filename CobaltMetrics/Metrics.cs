using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;

using CobaltMetrics.DataTypes.Generic;
using CobaltMetrics.DataTypes;

namespace CobaltMetrics
{
    public class Metrics
    {
        //Session Info
        private static Guid guid = Guid.NewGuid();
        private static long startTime;

        //Session Data
        private static List<IGenericData> metricData;

        //Metrics State
        private static bool running = false;
        private static bool locked = false;

        /// <summary>
        /// Creates a new metrics session, and begins accepting data.
        /// </summary>
        public static void StartMetrics()
        {
            if(running)
            {
                throw new InvalidOperationException("The metrics system is already running!");
            }

            if(locked)
            {
                throw new InvalidOperationException("The metrics system is currently locked, and is writing data. Consider a delay between metric sessions.");
            }

            if(!running && !locked)
            {
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                startTime = (int)t.TotalSeconds;

                metricData = new List<IGenericData>();
                running = true;
            }
        }

        /// <summary>
        /// Adds data to the current running metrics session. NOTE: This should only be called with your custom implementation of GenericData. Included types do this automatically.
        /// </summary>
        /// <param name="data">A data object implementing the GenericData interface.</param>
        public static void AddData(IGenericData data)
        {
            if(!running || locked)
            {
                throw new InvalidOperationException("There is no current metric session active/available.");
            }

            else if(running && !locked)
            {
                metricData.Add(data);
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
            int endTime = (int)t.TotalMilliseconds;

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(sb, xws))
            {
                XDocument doc = new XDocument();

                XElement session = new XElement("session");
                
                session.SetAttributeValue("sessionID", guid.ToString());
                session.SetAttributeValue("startTime", startTime.ToString());
                session.SetAttributeValue("endTime", endTime.ToString());
                session.SetAttributeValue("sessionTime", (endTime - startTime).ToString());

                foreach(IGenericData data in metricData)
                {
                    if (data.GetDataKey() != null && data.GetTimestamp() > startTime && data.GetDBDataValue() != null && data.GetDBDataValues() != null)
                    {
                        XElement dataElement = new XElement("data");
                        dataElement.SetAttributeValue("key", data.GetDataKey());
                        dataElement.SetAttributeValue("timestamp", data.GetTimestamp());
                        dataElement.SetAttributeValue("type", data.GetDataType().ToString());

                        switch(data.GetDataType())
                        {
                            case DataType.SINGLE:
                            {
                                dataElement.SetElementValue("value", data.GetDBDataValue());
                                break;
                            }
                            case DataType.ARRAY:
                            {
                                for (int i = 0; i < data.GetDBDataValues().Count; ++i )
                                {
                                    dataElement.SetElementValue("value" + i.ToString(), data.GetDBDataValues()[i]);
                                }

                                break;
                            }
                        }
                    }
                }

                doc.Add(session);
                doc.Save(xw);
            }
        }
    }
}