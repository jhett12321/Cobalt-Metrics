using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CobaltMetrics.DataTypes.Generic
{
    /// <summary>
    /// Represents a generic incremental.
    /// </summary>
    public class GenericIncrement : IGenericData
    {
        private const DataType dataType = DataType.INCREMENT;

        //Generic Data  
        private string key;
        private long timestamp;

        //Singlular data
        private int data;

        /// <summary>
        /// Increments the given key on the server, creating it if it exists.
        /// </summary>
        /// <param name="key">A named key representing this value. Should represent the named variable given for value, and should be unique for the whole project.</param>
        /// <param name="value">The value to add to this key.</param>
        public GenericIncrement(string key, int incrementAmount)
        {
            this.key = key;
            this.data = incrementAmount;

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
            return null;
        }

        public string GetDBDataValue()
        {
            return null;
        }

        public int GetDBIncrementValue()
        {
            return data;
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
