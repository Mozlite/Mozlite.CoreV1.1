
namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// É¾³ýÅÅÐò²Ù×÷¡£
    /// </summary>
    public class DropSequenceOperation : MigrationOperation
    {
        /// <summary>
        /// Ãû³Æ¡£
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }

        /// <summary>
        /// ¼Ü¹¹¡£
        /// </summary>
        public virtual string Schema { get; [param: CanBeNull] set; }
    }
}
