using System;
using System.Linq;

namespace Mozlite.Extensions
{
    /// <summary>
    /// 对象扩展类。
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 将以“,”分割的整形字符串转换为数组。
        /// </summary>
        /// <param name="ids">整形字符串。</param>
        /// <returns>返回转换后的结果，如果转换失败或为空则会被过滤掉。</returns>
        public static int[] SplitToInt32(this string ids)
        {
            return ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.AsInt32())
                .Where(i => i != null)
                .Select(i => i.Value)
                .Distinct()
                .ToArray();
        }
    }
}