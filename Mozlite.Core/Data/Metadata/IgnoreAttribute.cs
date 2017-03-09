using System;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 忽略特性。
    /// </summary>
    /// <remarks>
    /// <see cref="IdentityAttribute"/>优先级比<see cref="KeyAttribute"/>高，<see cref="IgnoreAttribute"/>优先级最低。
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class IgnoreAttribute : Attribute
    {
        /// <summary>
        /// 忽略操作选项。
        /// </summary>
        /// <param name="ignore">操作选项。</param>
        public IgnoreAttribute(Ignore ignore = Ignore.Upsert)
        {
            Ignore = ignore;
        }

        /// <summary>
        /// 忽略操作选项。
        /// </summary>
        public Ignore Ignore { get; }
    }
}