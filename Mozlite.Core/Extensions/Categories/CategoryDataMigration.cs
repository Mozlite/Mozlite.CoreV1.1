using Mozlite.Data.Migrations;
using Mozlite.Data.Migrations.Builders;

namespace Mozlite.Extensions.Categories
{
    /// <summary>
    /// 分类数据迁移积累。
    /// </summary>
    /// <typeparam name="TCategory">分类类型。</typeparam>
    public abstract class CategoryDataMigration<TCategory> : DataMigration<TCategory>
        where TCategory : CategoryBase
    {
        /// <summary>
        /// 创建操作。
        /// </summary>
        /// <param name="builder">迁移构建实例对象。</param>
        protected override void Create(MigrationBuilder<TCategory> builder)
        {
            builder.CreateTable(table =>
            {
                table.Column(x => x.Id)
                    .Column(x => x.Description)
                    .Column(x => x.Name, nullable: false)
                    .UniqueConstraint(x => x.Name);
                Create(table);
            });
        }

        /// <summary>
        /// 添加表格列。
        /// </summary>
        /// <param name="table">表格构建实例。</param>
        protected virtual void Create(CreateTableBuilder<TCategory> table) { }
    }
}