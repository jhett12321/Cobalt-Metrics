using System;
using System.Collections.Generic;

using CobaltMetrics.DataTypes.Generic;

namespace CobaltMetrics.DataTypes
{
    /// <summary>
    /// Represents a primitive.
    /// </summary>
    public class CMetricNativeIncrement
    {
        /// <summary>
        /// Creates and stores the given byte.
        /// </summary>
        /// <param name="key">A named key representing this byte. Should represent the named variable this byte is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The byte value for this key.</param>
        public static void Byte(string key, byte value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given sbyte.
        /// </summary>
        /// <param name="key">A named key representing this sbyte. Should represent the named variable this sbyte is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The sbyte value for this key.</param>
        public static void SByte(string key, sbyte value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given short.
        /// </summary>
        /// <param name="key">A named key representing this short. Should represent the named variable this short is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The short value for this key.</param>
        public static void Short(string key, short value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given ushort.
        /// </summary>
        /// <param name="key">A named key representing this ushort. Should represent the named variable this ushort is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The ushort value for this key.</param>
        public static void UShort(string key, ushort value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given int.
        /// </summary>
        /// <param name="key">A named key representing this int. Should represent the named variable this int is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The int value for this key.</param>
        public static void Int(string key, int value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given uint.
        /// </summary>
        /// <param name="key">A named key representing this uint. Should represent the named variable this uint is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The uint value for this key.</param>
        public static void UInt(string key, uint value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given long.
        /// </summary>
        /// <param name="key">A named key representing this long. Should represent the named variable this long is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The long value for this key.</param>
        public static void Long(string key, long value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given ulong.
        /// </summary>
        /// <param name="key">A named key representing this ulong. Should represent the named variable this ulong is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The ulong value for this key.</param>
        public static void ULong(string key, ulong value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given float.
        /// </summary>
        /// <param name="key">A named key representing this float. Should represent the named variable this float is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The float value for this key.</param>
        public static void Float(string key, float value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given double.
        /// </summary>
        /// <param name="key">A named key representing this double. Should represent the named variable this double is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The double value for this key.</param>
        public static void Double(string key, double value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given decimal.
        /// </summary>
        /// <param name="key">A named key representing this decimal. Should represent the named variable this decimal is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The decimal value for this key.</param>
        public static void Decimal(string key, decimal value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given char.
        /// </summary>
        /// <param name="key">A named key representing this char. Should represent the named variable this char is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The char value for this key.</param>
        public static void Char(string key, char value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }

        /// <summary>
        /// Creates and stores the given string.
        /// </summary>
        /// <param name="key">A named key representing this string. Should represent the named variable this string is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The string value for this key.</param>
        public static void String(string key)
        {
            new GenericIncrement(key, 1);
        }

        /// <summary>
        /// Creates and stores the given bool.
        /// </summary>
        /// <param name="key">A named key representing this bool. Should represent the named variable this bool is assigned to, and should be unique for the whole project.</param>
        /// <param name="value">The bool value for this key.</param>
        public static void Bool(string key, bool value)
        {
            new GenericIncrement(key, Convert.ToInt32(value));
        }
    }
}