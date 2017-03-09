using Mozlite.Data.Metadata;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 删除唯一约束。
    /// </summary>
    public class DropUniqueConstraintOperation : MigrationOperation
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }
        
        /// <summary>
        /// 表格。
        /// </summary>
        public virtual ITable Table { get; [param: NotNull] set; }
    }
}
