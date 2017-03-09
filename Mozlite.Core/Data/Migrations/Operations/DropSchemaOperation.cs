
namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// É¾³ý¼Ü¹¹¡£
    /// </summary>
    public class DropSchemaOperation : MigrationOperation
    {
        /// <summary>
        /// Ãû³Æ¡£
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }
    }
}
