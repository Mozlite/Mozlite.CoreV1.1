using Mozlite.Data.Migrations.Builders;
using Mozlite.Extensions.Categories;

namespace Mozlite.Extensions.Channels
{
    /// <summary>
    /// 频道数据库迁移类。
    /// </summary>
    public abstract class ChannelDataMigration : CategoryDataMigration<Channel>
    {
        /// <summary>
        /// 添加表格列。
        /// </summary>
        /// <param name="table">表格构建实例。</param>
        protected override void Create(CreateTableBuilder<Channel> table)
        {
            base.Create(table);
            table.Column(x => x.IconName)
                .Column(x => x.ClassName)
                .Column(x => x.DisplayName)
                .Column(x => x.LinkUrl)
                .Column(x => x.LinkTarget)
                .Column(x => x.Priority)
                .Column(x => x.Disabled);
        }
    }
}