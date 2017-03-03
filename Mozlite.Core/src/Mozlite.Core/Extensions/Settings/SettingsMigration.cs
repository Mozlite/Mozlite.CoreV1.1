using Mozlite.Data.Migrations;

namespace Mozlite.Extensions.Settings
{
    /// <summary>
    /// 数据库迁移。
    /// </summary>
    public class SiteSettingsMigration : DataMigration<SettingsAdapter>
    {
        /// <summary>
        /// 创建操作。
        /// </summary>
        /// <param name="builder">迁移构建实例对象。</param>
        protected override void Create(MigrationBuilder<SettingsAdapter> builder)
        {
            builder.CreateTable(table => table
                .Column(s => s.SettingsId)
                .Column(s => s.SettingsValue)
            );
        }
    }
}