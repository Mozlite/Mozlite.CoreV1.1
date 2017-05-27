using Mozlite.Data.Metadata;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 删除主键。
    /// </summary>
    public class DropPrimaryKeyOperation : MigrationOperation
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }

        /// <summary>
        /// 表格。
        /// </summary>
        public virtual ITable Table { get; [param: CanBeNull] set; }
    }
}
