using System;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 大小特性。
    /// </summary>
    public class SizeAttribute : Attribute
    {
        /// <summary>
        /// 大小。
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// 初始化类<see cref="SizeAttribute"/>。
        /// </summary>
        /// <param name="size">大小。</param>
        public SizeAttribute(int size)
        {
            Size = size;
        }
    }
}