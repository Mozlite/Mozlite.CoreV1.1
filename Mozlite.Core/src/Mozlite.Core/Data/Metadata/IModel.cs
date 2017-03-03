using System;
using Mozlite.Core;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 模型接口。
    /// </summary>
    public interface IModel : ISingletonService
    {
        /// <summary>
        /// 通过类型获取实体类型。
        /// </summary>
        /// <param name="type">当前类型实例。</param>
        /// <returns>返回实体类型。</returns>
        IEntityType GetEntity(Type type);
        
        /// <summary>
        /// 获取表格。
        /// </summary>
        /// <param name="type">当前类型。</param>
        /// <returns>返回表格。</returns>
        ITable GetTable(Type type);

        /// <summary>
        /// 实例化一个表格，不进行缓存。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <returns>返回表格实例。</returns>
        ITable GetTable(string name, string schema);

        /// <summary>
        /// 通过表格名称和架构获取当前实体。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <returns>返回实体对象。</returns>
        IEntityType FindEntity(string name, string schema);
    }
}