using System;

namespace Mozlite.Extensions
{
    /// <summary>
    /// 包含Id的对象接口。
    /// </summary>
    public interface IObjectBase
    {
        /// <summary>
        /// 唯一ID，一般为自增长。
        /// </summary>
        int Id { get; set; }
    }
}