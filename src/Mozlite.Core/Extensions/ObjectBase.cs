using Mozlite.Data.Metadata;

namespace Mozlite.Extensions
{
    /// <summary>
    /// 包含ID的对象基类。
    /// </summary>
    public abstract class ObjectBase : IObjectBase
    {
        /// <summary>
        /// 唯一ID，一般为自增长。
        /// </summary>
        [Identity]
        public int Id { get; set; }
    }
}