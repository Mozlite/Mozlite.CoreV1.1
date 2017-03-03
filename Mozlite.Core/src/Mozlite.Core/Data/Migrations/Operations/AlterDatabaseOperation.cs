using Mozlite.Core;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 修改数据库操作。
    /// </summary>
    public class AlterDatabaseOperation : MigrationOperation, IAlterMigrationOperation
    {
        /// <summary>
        /// 原数据库的一些配置属性。
        /// </summary>
        public virtual Annotatable OldDatabase { get; } = new Annotatable();

        IMutableAnnotatable IAlterMigrationOperation.OldAnnotations => OldDatabase;
    }
}
