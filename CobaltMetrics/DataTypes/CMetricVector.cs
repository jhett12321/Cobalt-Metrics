using System;
using System.Collections.Generic;
using UnityEngine;

using CobaltMetrics.DataTypes.Generic;

namespace CobaltMetrics.DataTypes
{
    /// <summary>
    /// Represents a 2D 3D and 4D Unity Vector, or a List<float> representing an x size vector.
    /// </summary>
    public class CMetricVector
    {
        /// <summary>
        /// Creates and stores the given vector.
        /// </summary>
        /// <param name="key">A named key representing this value. Should represent the named variable given for value, and should be unique for the whole project.</param>
        /// <param name="value">The value for this key.</param>
        public static void Create(string key, Vector2 value)
        {
            List<string> list = new List<string>();

            list.Add(value.x.ToString());
            list.Add(value.y.ToString());

            new GenericList(key, list);
        }

        /// <summary>
        /// Creates and stores the given vector.
        /// </summary>
        /// <param name="key">A named key representing this value. Should represent the named variable given for value, and should be unique for the whole project.</param>
        /// <param name="value">The value for this key.</param>
        public static void Create(string key, Vector3 value)
        {
            List<string> list = new List<string>();

            list.Add(value.x.ToString());
            list.Add(value.y.ToString());
            list.Add(value.z.ToString());

            new GenericList(key, list);
        }

        /// <summary>
        /// Creates and stores the given vector.
        /// </summary>
        /// <param name="key">A named key representing this value. Should represent the named variable given for value, and should be unique for the whole project.</param>
        /// <param name="value">The value for this key.</param>
        public static void Create(string key, Vector4 value)
        {
            List<string> list = new List<string>();

            list.Add(value.x.ToString());
            list.Add(value.y.ToString());
            list.Add(value.z.ToString());
            list.Add(value.w.ToString());

            new GenericList(key, list);
        }

        /// <summary>
        /// Creates and stores the given vector.
        /// </summary>
        /// <param name="key">A named key representing this value. Should represent the named variable given for value, and should be unique for the whole project.</param>
        /// <param name="value">The value for this key.</param>
        public static void Create(string key, List<float> value)
        {
            List<string> list = new List<string>();

            foreach (float f in value)
            {
                list.Add(f.ToString());
            }

            new GenericList(key, list);
        }
    }
}