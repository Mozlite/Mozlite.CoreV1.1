using System.Collections.Generic;
using Mozlite.Core;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 主键或者唯一键接口。
    /// </summary>
    public interface IKey : IAnnotatable
    {
        /// <summary>
        /// 所包含的属性。
        /// </summary>
        IReadOnlyList<IProperty> Properties { get; }

        /// <summary>
        /// 所属类型。
        /// </summary>
        IEntityType DeclaringType { get; }
    }
}