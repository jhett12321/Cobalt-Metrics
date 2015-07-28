using System.Collections.Generic;

namespace CobaltMetrics.DataTypes.Generic
{
    public interface IGenericData
    {
        string GetDBDataKey();
        string GetDBDataValue();
        List<string> GetDBDataValues();
        int GetTimestamp();
    }
}