using Mozlite.Core;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 迁移数据操作基类。
    /// </summary>
    public abstract class MigrationOperation : Annotatable
    {
        /// <summary>
        /// 更改是否会造成不可挽回的破坏。
        /// </summary>
        public virtual bool IsDestructiveChange { get; set; }
    }
}
