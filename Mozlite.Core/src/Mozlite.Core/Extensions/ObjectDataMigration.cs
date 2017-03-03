using Mozlite.Data.Migrations;
using Mozlite.Data.Migrations.Builders;

namespace Mozlite.Extensions
{
    /// <summary>
    /// 对象数据迁移基类。
    /// </summary>
    /// <typeparam name="TModel">实体类型。</typeparam>
    public abstract class ObjectDataMigration<TModel> : DataMigration<TModel>
        where TModel : ExtendObjectBase
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        protected override void Create(MigrationBuilder<TModel> builder)
        {
            builder.CreateTable(table =>
            {
                table
                    .Column(x => x.Id)
                    .Column(x => x.Guid)
                    .Column(x => x.Key)
                    .Column(x => x.CreatedDate)
                    .Column(x => x.UpdatedDate)
                    .Column(x => x.Status)
                    .Column(x => x.ExtendProperties);
                Create(table);
            });
        }

        /// <summary>
        /// 添加模型对象。
        /// </summary>
        /// <param name="table">添加表格的构建实例对象。</param>
        protected abstract void Create(CreateTableBuilder<TModel> table);
    }
}