using Mozlite.Data.Metadata;

namespace Mozlite.Extensions.Searching
{
    /// <summary>
    /// 索引和搜索实体关联表。
    /// </summary>
    [Table("Searching_In_Indexes")]
    public class SearchInIndex
    {
        /// <summary>
        /// 检索实体Id。
        /// </summary>
        [Key]
        public long SearchId { get; set; }

        /// <summary>
        /// 索引ID。
        /// </summary>
        [Key]
        public long IndexId { get; set; }
    }
}