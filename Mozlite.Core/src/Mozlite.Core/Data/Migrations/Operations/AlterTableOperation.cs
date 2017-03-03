using Mozlite.Core;
using Mozlite.Data.Metadata;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 修改表格。
    /// </summary>
    public class AlterTableOperation : MigrationOperation, IAlterMigrationOperation
    {
        /// <summary>
        /// 表格名称。
        /// </summary>
        public virtual ITable Table { get; [param: NotNull] set; }

        /// <summary>
        /// 原表格的相关属性。
        /// </summary>
        public virtual Annotatable OldTable { get; [param: NotNull] set; } = new Annotatable();

        IMutableAnnotatable IAlterMigrationOperation.OldAnnotations => OldTable;
    }
}
