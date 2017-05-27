using Mozlite.Data.Metadata;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 删除列。
    /// </summary>
    public class DropColumnOperation : MigrationOperation
    {
        /// <summary>
        /// 初始化类<see cref="DropColumnOperation"/>。
        /// </summary>
        public DropColumnOperation()
        {
            IsDestructiveChange = true;
        }

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
