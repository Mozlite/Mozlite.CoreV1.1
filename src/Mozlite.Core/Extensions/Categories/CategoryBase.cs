using Mozlite.Data.Metadata;

namespace Mozlite.Extensions.Categories
{
    /// <summary>
    /// 分类基类。
    /// </summary>
    public abstract class CategoryBase
    {
        /// <summary>
        /// 分类Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 唯一名称。
        /// </summary>
        [Size(20)]
        public string Name { get; set; }
        
        /// <summary>
        /// 描述。
        /// </summary>
        [Size(256)]
        public string Description { get; set; }
    }
}