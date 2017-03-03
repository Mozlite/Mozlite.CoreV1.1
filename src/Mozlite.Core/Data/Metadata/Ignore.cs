using System;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 忽略操作枚举。
    /// </summary>
    [Flags]
    public enum Ignore
    {
        /// <summary>
        /// 无。
        /// </summary>
        None = 0,
        /// <summary>
        /// 插入。
        /// </summary>
        Insert = 1,
        /// <summary>
        /// 更新。
        /// </summary>
        Update = 2,
        /// <summary>
        /// 插入和更新。
        /// </summary>
        Upsert = Insert | Update,
        /// <summary>
        /// 列表。
        /// </summary>
        List = 4,
        /// <summary>
        /// 忽略所有。
        /// </summary>
        All = Upsert | List,
    }
}