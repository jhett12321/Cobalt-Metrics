using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

using CobaltMetrics.DataTypes.Generic;

namespace CobaltMetrics
{
    public class Metrics
    {
        private static Guid guid = Guid.NewGuid();
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

            foreach(IGenericData data in metricData)
            {
                if(data.GetDBDataValue() != null)
                {
                    
                }
            }

            //XDocument doc = new XDocument();
            //doc.Add(new XElement(guid.ToString(), metricData.Select(x =>new XElement(x.GetDBDataKey(), x.GetDBDataValues()))));
        }
    }
}