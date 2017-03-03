using Mozlite.Data.Metadata;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 修改表格名称。
    /// </summary>
    public class RenameTableOperation : MigrationOperation
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public virtual ITable Table { get; [param: NotNull] set; }
        
        /// <summary>
        /// 新名称。
        /// </summary>
        public virtual ITable NewTable { get; [param: CanBeNull] set; }
    }
}
