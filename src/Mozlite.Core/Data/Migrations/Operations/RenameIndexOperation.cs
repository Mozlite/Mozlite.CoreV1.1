using Mozlite.Data.Metadata;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 索引名称更改操作。
    /// </summary>
    public class RenameIndexOperation : MigrationOperation
    {
        /// <summary>
        /// 原有名称。
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }

        /// <summary>
        /// 新名称。
        /// </summary>
        public virtual string NewName { get; [param: NotNull] set; }
        
        /// <summary>
        /// 表格名称。
        /// </summary>
        public virtual ITable Table { get; [param: CanBeNull] set; }
    }
}
