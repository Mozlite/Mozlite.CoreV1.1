using Mozlite.Data.Metadata;

namespace Mozlite.Extensions.Searching
{
    /// <summary>
    /// 搜索实例对象。
    /// </summary>
    [Table("core_Searching_Indexes")]
    public class SearchIndex
    {
        /// <summary>
        /// 索引ID。
        /// </summary>
        [Identity]
        public long Id { get; set; }

        /// <summary>
        /// 检索名称。
        /// </summary>
        [Key]
        [Size(64)]
        [Ignore(Ignore.Update)]
        public string Name { get; set; }

        /// <summary>
        /// 优先级，每检索一次加1。
        /// </summary>
        public int Priority { get; set; }
    }
}