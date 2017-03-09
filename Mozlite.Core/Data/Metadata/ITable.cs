using System;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 表格。
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 架构。
        /// </summary>
        string Schema { get; }

        /// <summary>
        /// 声明类型。
        /// </summary>
        Type DeclaringType { get; }
    }
}