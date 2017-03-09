using Mozlite.Data.Metadata;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 修改列名称。
    /// </summary>
    public class RenameColumnOperation : MigrationOperation
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }

        /// <summary>
        /// 表格。
        /// </summary>
        public virtual ITable Table { get; [param: CanBeNull] set; }
        /// <summary>
        /// 新名称。
        /// </summary>
        public virtual string NewName { get; [param: NotNull] set; }
    }
}
