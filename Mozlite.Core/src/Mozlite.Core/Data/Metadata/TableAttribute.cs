using System;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 表格特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 实体类型。
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// 初始化 <see cref="TableAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="tableName">表格名称。</param>
        /// <param name="schema">架构名称。</param>
        public TableAttribute(string tableName, string schema = null)
        {
            Name = tableName;
            Schema = schema;
        }

        /// <summary>
        /// 初始化 <see cref="TableAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="entityType">实体类型。</param>
        public TableAttribute(Type entityType)
        {
            EntityType = entityType;
        }

        /// <summary>
        /// 表格名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 架构名称。
        /// </summary>
        public string Schema { get; }
    }
}