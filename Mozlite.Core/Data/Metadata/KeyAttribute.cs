using System;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 主键或者唯一键，可用于单独查询标志。
    /// </summary>
    /// <remarks>
    /// <see cref="IdentityAttribute"/>优先级比<see cref="KeyAttribute"/>高，<see cref="IgnoreAttribute"/>优先级最低。
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
        /// <summary>
        /// 忽略操作方法。
        /// </summary>
        public Ignore Ignore { get; }

        /// <summary>
        /// 初始化类<see cref="KeyAttribute"/>，表示只新建不更新。
        /// </summary>
        public KeyAttribute() : this(Ignore.Update) { }

        /// <summary>
        /// 初始化类<see cref="KeyAttribute"/>。
        /// </summary>
        /// <param name="ignore">忽略操作方法。</param>
        public KeyAttribute(Ignore ignore)
        {
            Ignore = ignore;
        }
    }
}