using System;

namespace Mozlite.Extensions
{
    /// <summary>
    /// 对象接口。
    /// </summary>
    public interface IObject
    {
        /// <summary>
        /// 自增长唯一Id，主键。
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 全局唯一Id。
        /// </summary>
        Guid Guid { get; set; }

        /// <summary>
        /// 唯一键，一般用于URL查询，建立索引。
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// 状态。
        /// </summary>
        ObjectStatus Status { get; set; }
    }
}