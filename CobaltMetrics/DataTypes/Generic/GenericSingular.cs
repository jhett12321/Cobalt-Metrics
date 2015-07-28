using System;
using System.Collections.Generic;
using UnityEngine;

namespace CobaltMetrics.DataTypes.Generic
{
    /// <summary>
    /// Represents a generic list.
    /// </summary>
    public class GenericString : IGenericData
    {
        //Generic Data  
        private string key;
        private int timestamp;

        //Singlular data
        private string data;

        /// <summary>
        /// Creates and stores the given singular form.
        /// </summary>
        /// <param name="key">A named key representing this value. Should represent the named variable given for value, and should be unique for the whole project.</param>
        /// <param name="value">The value for this key.</param>
        public GenericString(string key, string value)
        {
            this.key = key;
            this.data = value;

            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            this.timestamp = (int)t.TotalSeconds;

            Metrics.AddData(this);
        }

        //Interface
        public string GetDBDataKey()
        {
            return key;
        }

        public List<string> GetDBDataValues()
        {
            return null;
        }

        public string GetDBDataValue()
        {
            return data;
        }

        public int GetTimestamp()
        {
            return timestamp;
        }
    }
}