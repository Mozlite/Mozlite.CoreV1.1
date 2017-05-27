using Mozlite.Data.Metadata;

namespace Mozlite.Data.Migrations.Operations
{
    /// <summary>
    /// 添加外键操作。
    /// </summary>
    public class AddForeignKeyOperation : MigrationOperation
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string Name { get; [param: NotNull] set; }
        
        /// <summary>
        /// 表格。
        /// </summary>
        public virtual ITable Table { get; [param: NotNull] set; }

        /// <summary>
        /// 相关列。
        /// </summary>
        public virtual string[] Columns { get; [param: NotNull] set; }
        
        /// <summary>
        /// 主键表。
        /// </summary>
        public virtual ITable PrincipalTable { get; [param: NotNull] set; }

        /// <summary>
        /// 主键列。
        /// </summary>
        public virtual string[] PrincipalColumns { get; [param: NotNull] set; }

        /// <summary>
        /// 主键更新时候的操作。
        /// </summary>
        public virtual ReferentialAction OnUpdate { get; set; }

        /// <summary>
        /// 主键删除时候的操作。
        /// </summary>
        public virtual ReferentialAction OnDelete { get; set; }
    }
}
