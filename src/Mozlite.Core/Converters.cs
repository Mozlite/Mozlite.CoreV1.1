using System;

namespace Mozlite
{
    /// <summary>
    /// 转换扩展类。
    /// </summary>
    public static class Converters
    {
        /// <summary>
        /// 转换字符串为布尔型或<c>null</c>。
        /// </summary>
        /// <param name="value">当前字符串实例对象。</param>
        /// <returns>返回转换结果。</returns>
        public static bool? AsBoolean(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Trim();
            bool result;
            if (bool.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// 转换字符串为整形或<c>null</c>。
        /// </summary>
        /// <param name="value">当前字符串实例对象。</param>
        /// <returns>返回转换结果。</returns>
        public static short? AsInt16(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Trim();
            short result;
            if (short.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// 转换字符串为整形或<c>null</c>。
        /// </summary>
        /// <param name="value">当前字符串实例对象。</param>
        /// <returns>返回转换结果。</returns>
        public static int? AsInt32(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Trim();
            int result;
            if (int.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// 转换字符串为长整形或<c>null</c>。
        /// </summary>
        /// <param name="value">当前字符串实例对象。</param>
        /// <returns>返回转换结果。</returns>
        public static long? AsInt64(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Trim();
            long result;
            if (long.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// 转换字符串为日期或<c>null</c>。
        /// </summary>
        /// <param name="value">当前字符串实例对象。</param>
        /// <returns>返回转换结果。</returns>
        public static DateTime? AsDateTime(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Trim();
            DateTime result;
            if (DateTime.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// 转换字符串为日期或<c>null</c>。
        /// </summary>
        /// <param name="value">当前字符串实例对象。</param>
        /// <returns>返回转换结果。</returns>
        public static DateTimeOffset? AsDateTimeOffset(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Trim();
            DateTimeOffset result;
            if (DateTimeOffset.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// 转换字符串为double类型或<c>null</c>。
        /// </summary>
        /// <param name="value">当前字符串实例对象。</param>
        /// <returns>返回转换结果。</returns>
        public static double? AsDouble(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Trim();
            double result;
            if (double.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// 转换字符串为float类型或<c>null</c>。
        /// </summary>
        /// <param name="value">当前字符串实例对象。</param>
        /// <returns>返回转换结果。</returns>
        public static float? AsFloat(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Trim();
            float result;
            if (float.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// 转换字符串为枚举类型或<c>null</c>。
        /// </summary>
        /// <param name="value">当前字符串实例对象。</param>
        /// <returns>返回转换结果。</returns>
        public static T? AsEnum<T>(this string value)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Trim();
            T result;
            if (Enum.TryParse(value, true, out result))
                return result;
            return null;
        }

        /// <summary>
        /// 转换字符串为Guid类型或<c>null</c>。
        /// </summary>
        /// <param name="value">当前字符串实例对象。</param>
        /// <returns>返回转换结果。</returns>
        public static Guid? AsGuid(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Trim();
            Guid result;
            if (Guid.TryParse(value, out result))
                return result;
            return null;
        }

        /// <summary>
        /// 转换字符串为decimal类型或<c>null</c>。
        /// </summary>
        /// <param name="value">当前字符串实例对象。</param>
        /// <returns>返回转换结果。</returns>
        public static decimal? AsDecimal(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Trim();
            decimal result;
            if (decimal.TryParse(value, out result))
                return result;
            return null;
        }
    }
}