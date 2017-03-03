using Mozlite.Core;
using Mozlite.Data.Metadata;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 修改列。
    /// </summary>
    public class AlterColumnOperation : ColumnOperation, IAlterMigrationOperation
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }
        
        /// <summary>
        /// 表格。
        /// </summary>
        public virtual ITable Table { get; [param: NotNull] set; }

        /// <summary>
        /// 原来列的配置属性。
        /// </summary>
        public virtual ColumnOperation OldColumn { get; [param: NotNull] set; } = new ColumnOperation();

        IMutableAnnotatable IAlterMigrationOperation.OldAnnotations => OldColumn;
    }
}
