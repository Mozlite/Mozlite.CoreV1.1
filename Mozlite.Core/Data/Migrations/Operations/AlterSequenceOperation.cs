using Mozlite.Core;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 修改排序操作。
    /// </summary>
    public class AlterSequenceOperation : SequenceOperation, IAlterMigrationOperation
    {
        /// <summary>
        /// 架构。
        /// </summary>
        public virtual string Schema { get; [param: CanBeNull] set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }

        /// <summary>
        /// 原来的相关属性。
        /// </summary>
        public virtual SequenceOperation OldSequence { get; [param: NotNull] set; } = new SequenceOperation();

        IMutableAnnotatable IAlterMigrationOperation.OldAnnotations => OldSequence;
    }
}
