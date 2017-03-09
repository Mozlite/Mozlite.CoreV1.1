using System;
using System.Collections.Generic;
using System.Data.Common;
using Mozlite.Core;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 实体类型接口。
    /// </summary>
    public interface IEntityType : IAnnotatable
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 类型。
        /// </summary>
        Type ClrType { get; }

        /// <summary>
        /// 获取主键。
        /// </summary>
        IKey PrimaryKey { get; }

        /// <summary>
        /// 自增长列。
        /// </summary>
        IProperty Identity { get; }

        /// <summary>
        /// 通过名称查找属性实例。
        /// </summary>
        /// <param name="name">属性名称。</param>
        /// <returns>返回属性实例对象。</returns>
        IProperty FindProperty([NotNull] string name);

        /// <summary>
        /// 根据忽略特性获取属性集合。
        /// </summary>
        /// <param name="ignore">忽略特性。</param>
        /// <returns>返回属性集合。</returns>
        IEnumerable<IProperty> FindProperties(Ignore ignore);

        /// <summary>
        /// 获取当前类型的所有属性列表。
        /// </summary>
        /// <returns>所有属性列表。</returns>
        IEnumerable<IProperty> GetProperties();

        /// <summary>
        /// 从数据库读取器中读取当前实例对象。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="reader">数据库读取器。</param>
        /// <returns>返回当前模型实例对象。</returns>
        TModel Read<TModel>(DbDataReader reader);
    }
}