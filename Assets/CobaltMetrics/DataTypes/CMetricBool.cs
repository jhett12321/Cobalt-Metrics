using System;
using System.Collections.Generic;
using UnityEngine;

using CobaltMetrics.DataTypes.Generic;

namespace CobaltMetrics.DataTypes
{
    /// <summary>
    /// Represents a bool primitive.
    /// </summary>
    public class CMetricBool
    {
        /// <summary>
        /// Creates and stores the given bool.
        /// </summary>
        /// <param name="key">A named key representing this value. Should represent the named variable given for value, and should be unique for the whole project.</param>
        /// <param name="value">The value for this key.</param>
        public static void Create(string key, bool value)
        {
            new GenericString(key, value.ToString().ToLower());
        }
    }
}