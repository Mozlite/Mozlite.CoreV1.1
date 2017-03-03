using Mozlite.Data.Metadata;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 添加主键。
    /// </summary>
    public class AddPrimaryKeyOperation : MigrationOperation
    {
        /// <summary>
        /// 表格。
        /// </summary>
        public virtual ITable Table { get; [param: NotNull] set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }

        /// <summary>
        /// 相关列。
        /// </summary>
        public virtual string[] Columns { get; [param: NotNull] set; }
    }
}
