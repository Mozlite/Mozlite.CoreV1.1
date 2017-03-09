using Mozlite.Core;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 修改迁移操作。
    /// </summary>
    public interface IAlterMigrationOperation
    {
        /// <summary>
        /// 原有扩展实例。
        /// </summary>
        IMutableAnnotatable OldAnnotations { get; }
    }
}
