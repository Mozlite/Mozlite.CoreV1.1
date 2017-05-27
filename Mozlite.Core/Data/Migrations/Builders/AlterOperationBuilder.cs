using Mozlite.Data.Migrations.Operations;

namespace Mozlite.Data.Migrations.Builders
{
    /// <summary>
    /// 修改操作构建实例。
    /// </summary>
    /// <typeparam name="TOperation">操作类型。</typeparam>
    public class AlterOperationBuilder<TOperation> : OperationBuilder<TOperation>
        where TOperation : MigrationOperation, IAlterMigrationOperation
    {
        /// <summary>
        /// 初始化类<see cref="AlterOperationBuilder{TOperation}"/>。
        /// </summary>
        /// <param name="operation">操作实例对象。</param>
        public AlterOperationBuilder([NotNull] TOperation operation)
            : base(operation)
        {
        }

        /// <summary>
        /// 添加扩展实例。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="value">值。</param>
        /// <returns>返回当前操作构建实例对象。</returns>
        public new virtual AlterOperationBuilder<TOperation> Annotation(
                [NotNull] string name,
                [NotNull] object value)
            => (AlterOperationBuilder<TOperation>)base.Annotation(name, value);
        
        /// <summary>
        /// 添加原有扩展实例。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="value">值。</param>
        /// <returns>返回当前操作构建实例对象。</returns>
        public virtual AlterOperationBuilder<TOperation> OldAnnotation(
            [NotNull] string name,
            [NotNull] object value)
        {
            Check.NotEmpty(name, nameof(name));
            Check.NotNull(value, nameof(value));

            Operation.OldAnnotations.AddAnnotation(name, value);

            return this;
        }
    }
}
