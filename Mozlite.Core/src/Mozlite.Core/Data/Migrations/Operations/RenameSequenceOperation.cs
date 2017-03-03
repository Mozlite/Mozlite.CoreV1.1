
namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 修改排序名称。
    /// </summary>
    public class RenameSequenceOperation : MigrationOperation
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }

        /// <summary>
        /// 架构。
        /// </summary>
        public virtual string Schema { get; [param: CanBeNull] set; }

        /// <summary>
        /// 新名称。
        /// </summary>
        public virtual string NewName { get; [param: CanBeNull] set; }

        /// <summary>
        /// 新架构。
        /// </summary>
        public virtual string NewSchema { get; [param: CanBeNull] set; }
    }
}
