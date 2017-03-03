using System.Collections.Generic;
using Mozlite.Core;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 外键接口。
    /// </summary>
    public interface IForeignKey : IAnnotatable
    {
        /// <summary>
        /// 所属类型。
        /// </summary>
        IEntityType DeclaringType { get; }

        /// <summary>
        /// 属性列表。
        /// </summary>
        IReadOnlyList<IProperty> Properties { get; }

        /// <summary>
        /// 主键所在的属性类型。
        /// </summary>
        IEntityType PrincipalType { get; }

        /// <summary>
        /// 主键。
        /// </summary>
        IKey PrincipalKey { get; }
        
        /// <summary>
        /// 是否唯一。
        /// </summary>
        bool IsUnique { get; }

        /// <summary>
        /// 是否必须。
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        /// 删除操作方式。
        /// </summary>
        DeleteBehavior DeleteBehavior { get; }
    }
}