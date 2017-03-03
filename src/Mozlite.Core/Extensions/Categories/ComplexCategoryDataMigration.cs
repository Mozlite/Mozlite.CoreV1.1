using Mozlite.Data.Migrations;
using Mozlite.Data.Migrations.Builders;

namespace Mozlite.Extensions.Categories
{
    /// <summary>
    /// 多级分类数据迁移基类。
    /// </summary>
    /// <typeparam name="TCategory">分类类型。</typeparam>
    public abstract class ComplexCategoryDataMigration<TCategory> : CategoryDataMigration<TCategory>
        where TCategory : ComplexCategoryBase<TCategory>, new()
    {
        /// <summary>
        /// 添加表格列。
        /// </summary>
        /// <param name="table">表格构建实例。</param>
        protected override void Create(CreateTableBuilder<TCategory> table)
        {
            table.Column(x => x.ParentId);
        }
    }
}