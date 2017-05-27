using System;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 添加排序操作。
    /// </summary>
    public class CreateSequenceOperation : SequenceOperation
    {
        /// <summary>
        /// 架构。
        /// </summary>
        public virtual string Schema { get; [param: CanBeNull] set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }

        /// <summary>
        /// 当前类型。
        /// </summary>
        public virtual Type ClrType { get; [param: NotNull] set; }

        /// <summary>
        /// 开始值。
        /// </summary>
        public virtual long StartValue { get; set; } = 1L;
    }
}
