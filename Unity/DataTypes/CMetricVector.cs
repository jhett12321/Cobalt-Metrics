using System;
using System.Collections.Generic;
using UnityEngine;

using CobaltMetrics.DataTypes.Generic;

namespace CobaltMetrics.DataTypes.Unity
{
    /// <summary>
    /// Represents a 2D 3D and 4D Unity Vector, or a List<float> representing an x size vector.
    /// </summary>
    public class CMetricVector
    {
        /// <summary>
        /// Creates and stores the given Vector2.
        /// </summary>
        /// <param name="key">A named key representing this Vector2. Should represent the named variable this Vector2 is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The Vector2 value for this key.</param>
        public static void Vector2(string key, Vector2 value)
        {
            List<string> list = new List<string>();

            list.Add(value.x.ToString());
            list.Add(value.y.ToString());

            new GenericList(key, list);
        }

        /// <summary>
        /// Creates and stores the given Vector3.
        /// </summary>
        /// <param name="key">A named key representing this Vector3. Should represent the named variable this Vector3 is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The Vector3 value for this key.</param>
        public static void Vector3(string key, Vector3 value)
        {
            List<string> list = new List<string>();

            list.Add(value.x.ToString());
            list.Add(value.y.ToString());
            list.Add(value.z.ToString());

            new GenericList(key, list);
        }

        /// <summary>
        /// Creates and stores the given Vector4.
        /// </summary>
        /// <param name="key">A named key representing this Vector4. Should represent the named variable this Vector4 is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The Vector4 value for this key.</param>
        public static void Vector4(string key, Vector4 value)
        {
            List<string> list = new List<string>();

            list.Add(value.x.ToString());
            list.Add(value.y.ToString());
            list.Add(value.z.ToString());
            list.Add(value.w.ToString());

            new GenericList(key, list);
        }

        /// <summary>
        /// Creates and stores the given list.
        /// </summary>
        /// <param name="key">A named key representing this list. Should represent the named variable this list is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The list value for this key.</param>
        public static void VectorX(string key, List<float> value)
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