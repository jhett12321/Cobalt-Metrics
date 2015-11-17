using System.Collections.Generic;
using CobaltMetrics.DataTypes;

namespace CobaltMetrics.DataTypes.Generic
{
    public interface IGenericData
    {
        string GetDataKey();
        string GetDBDataValue();
        List<string> GetDBDataValues();
        int GetDBIncrementValue();

        DataType GetDataType();

        /// <summary>
        /// Gets the timestamp for this data entry.
        /// </summary>
        /// <returns>A timestamp since epoch, measured in milliseconds.</returns>
        long GetTimestamp();
    }
}