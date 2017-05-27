using Mozlite.Data.Metadata;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 删除表格。
    /// </summary>
    public class DropTableOperation : MigrationOperation
    {
        /// <summary>
        /// 初始化类<see cref="DropTableOperation"/>。
        /// </summary>
        public DropTableOperation()
        {
            IsDestructiveChange = true;
        }

        /// <summary>
        /// 名称。
        /// </summary>
        public virtual ITable Table { get; [param: NotNull] set; }
    }
}
