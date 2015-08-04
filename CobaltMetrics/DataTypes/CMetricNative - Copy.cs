//using System;
//using System.Collections.Generic;

//using CobaltMetrics.DataTypes.Generic;

//namespace CobaltMetrics.DataTypes
//{
//    /// <summary>
//    /// Represents a native C# data type.
//    /// </summary>
//    public class CMetricNative
//    {
//        private static List<string> watchedByteKeys = new List<string>();
//        private static List<object> watchedByteValues = new List<object>();
//        private static List<float> watchedByteTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given byte.
//        /// </summary>
//        /// <param name="key">A named key representing this byte. Should represent the named variable this byte is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The byte value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void Byte(string key, byte value, float? watchTime)
//        {
//            new GenericString(key, value.ToString());

//            if(watchTime != null)
//            {
//                if (!watchedByteKeys.Contains(key))
//                {
//                    watchedByteKeys.Add(key);
//                    watchedByteValues.Add(value);
//                    watchedByteTimes.Add(watchTime.Value);
//                }
//            }
//        }

//        private static List<string> watchedSByteKeys = new List<string>();
//        private static List<object> watchedSByteValues = new List<object>();
//        private static List<float> watchedSByteTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given sbyte.
//        /// </summary>
//        /// <param name="key">A named key representing this sbyte. Should represent the named variable this sbyte is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The sbyte value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void SByte(string key, sbyte value, ref float? watchTime)
//        {
//            new GenericString(key, value.ToString());
//        }

//        private static List<string> watchedShortKeys = new List<string>();
//        private static List<object> watchedShortValues = new List<object>();
//        private static List<float> watchedShortTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given short.
//        /// </summary>
//        /// <param name="key">A named key representing this short. Should represent the named variable this short is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The short value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void Short(string key, short value, float? watchTime)
//        {
//            new GenericString(key, value.ToString());
//        }

//        private static List<string> watchedUShortKeys = new List<string>();
//        private static List<object> watchedUShortValues = new List<object>();
//        private static List<float> watchedUShortTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given ushort.
//        /// </summary>
//        /// <param name="key">A named key representing this ushort. Should represent the named variable this ushort is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The ushort value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void UShort(string key, ushort value, float? watchTime)
//        {
//            new GenericString(key, value.ToString());
//        }

//        private static List<string> watchedIntKeys = new List<string>();
//        private static List<object> watchedIntValues = new List<object>();
//        private static List<float> watchedIntTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given int.
//        /// </summary>
//        /// <param name="key">A named key representing this int. Should represent the named variable this int is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The int value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void Int(string key, int value, float? watchTime)
//        {
//            new GenericString(key, value.ToString());
//        }

//        private static List<string> watchedUIntKeys = new List<string>();
//        private static List<object> watchedUIntValues = new List<object>();
//        private static List<float> watchedUIntTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given uint.
//        /// </summary>
//        /// <param name="key">A named key representing this uint. Should represent the named variable this uint is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The uint value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void UInt(string key, uint value, float? watchTime)
//        {
//            new GenericString(key, value.ToString());
//        }

//        private static List<string> watchedLongKeys = new List<string>();
//        private static List<object> watchedLongValues = new List<object>();
//        private static List<float> watchedLongTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given long.
//        /// </summary>
//        /// <param name="key">A named key representing this long. Should represent the named variable this long is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The long value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void Long(string key, long value, float? watchTime)
//        {
//            new GenericString(key, value.ToString());
//        }

//        private static List<string> watchedULongKeys = new List<string>();
//        private static List<object> watchedULongValues = new List<object>();
//        private static List<float> watchedULongTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given ulong.
//        /// </summary>
//        /// <param name="key">A named key representing this ulong. Should represent the named variable this ulong is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The ulong value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void ULong(string key, ulong value, float? watchTime)
//        {
//            new GenericString(key, value.ToString());
//        }

//        private static List<string> watchedFloatKeys = new List<string>();
//        private static List<object> watchedFloatValues = new List<object>();
//        private static List<float> watchedFloatTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given float.
//        /// </summary>
//        /// <param name="key">A named key representing this float. Should represent the named variable this float is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The float value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void Float(string key, float value, float? watchTime)
//        {
//            new GenericString(key, value.ToString());
//        }

//        private static List<string> watchedDoubleKeys = new List<string>();
//        private static List<object> watchedDoubleValues = new List<object>();
//        private static List<float> watchedDoubleTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given double.
//        /// </summary>
//        /// <param name="key">A named key representing this double. Should represent the named variable this double is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The double value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void Double(string key, double value, float? watchTime)
//        {
//            new GenericString(key, value.ToString());
//        }

//        private static List<string> watchedDecimalKeys = new List<string>();
//        private static List<object> watchedDecimalValues = new List<object>();
//        private static List<float> watchedDecimalTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given decimal.
//        /// </summary>
//        /// <param name="key">A named key representing this decimal. Should represent the named variable this decimal is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The decimal value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void Decimal(string key, decimal value, float? watchTime)
//        {
//            new GenericString(key, value.ToString());
//        }

//        private static List<string> watchedCharKeys = new List<string>();
//        private static List<object> watchedCharValues = new List<object>();
//        private static List<float> watchedCharTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given char.
//        /// </summary>
//        /// <param name="key">A named key representing this char. Should represent the named variable this char is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The char value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void Char(string key, char value, float? watchTime)
//        {
//            new GenericString(key, value.ToString());
//        }

//        private static List<string> watchedStringKeys = new List<string>();
//        private static List<object> watchedStringValues = new List<object>();
//        private static List<float> watchedStringTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given string.
//        /// </summary>
//        /// <param name="key">A named key representing this string. Should represent the named variable this string is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The string value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void String(string key, string value, float? watchTime)
//        {
//            new GenericString(key, value);
//        }

//        private static List<string> watchedBoolKeys = new List<string>();
//        private static List<object> watchedBoolValues = new List<object>();
//        private static List<float> watchedBoolTimes = new List<float>();
//        /// <summary>
//        /// Creates and stores the given bool.
//        /// </summary>
//        /// <param name="key">A named key representing this bool. Should represent the named variable this bool is assigned to, and should be unique for the whole project.</param>
//        /// <param name="value">The bool value for this key.</param>
//        /// <param name="watchTime">The frequency to recheck this variable. Set this to null to run once (not recheck).</param>
//        public static void Bool(string key, bool value, float? watchTime)
//        {
//            new GenericString(key, value.ToString().ToLower());
//        }

//        public static void Update(float deltaT)
//        {

//        }
//    }
//}