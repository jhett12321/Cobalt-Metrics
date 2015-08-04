using System;
using System.Collections.Generic;

namespace CobaltMetrics.DataTypes.Generic
{
    /// <summary>
    /// Represents a generic list.
    /// </summary>
    public class GenericList : IGenericData
    {
        private const DataType dataType = DataType.ARRAY;

        //Generic Data  
        private string key;
        private long timestamp;

        //List Data
        private List<string> data = new List<string>();

        /// <summary>
        /// Creates and stores the given array.
        /// </summary>
        /// <param name="key">A named key representing this value. Should represent the named variable given for array, and should be unique for the whole project.</param>
        /// <param name="array">The value for this key.</param>
        public GenericList(string key, List<string> value)
        {
            this.key = key;

            //TODO AddRange or simply make data equal to array?
            data.AddRange(value);

            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            this.timestamp = (long)t.TotalMilliseconds;

            Metrics.AddData(this);
        }

        //Interface
        public string GetDataKey()
        {
            return key;
        }

        public List<string> GetDBDataValues()
        {
            return data;
        }

        public string GetDBDataValue()
        {
            return null;
        }

        public DataType GetDataType()
        {
            return dataType;
        }

        public long GetTimestamp()
        {
            return timestamp;
        }
    }
}