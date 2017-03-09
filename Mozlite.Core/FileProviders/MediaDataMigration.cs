using Mozlite.Data.Migrations;
namespace Mozlite.FileProviders
{
    /// <summary>
    /// 媒体文件存储数据库迁移类。
    /// </summary>
    public class MediaDataMigration : DataMigration
    {
        /// <inheritdoc />
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<StorageFileInfo>(table => table
                .Column(x => x.FileId)
                .Column(x => x.Name, nullable: false)
                .Column(x => x.Length)
                .Column(x => x.ContentType, nullable: false)
                .Column(x => x.Reference)
                .UniqueConstraint(x => x.Name)
            );

            builder.CreateTable<MediaFileInfo>(table => table
                .Column(x => x.Id)
                .Column(x => x.Title)
                .Column(x => x.CreatedDate)
                .Column(x => x.Extension)
                .Column(x => x.FileId, defaultValue: 0)
                .Column(x => x.Length)
                .Column(x => x.Type, nullable: false)
                .Column(x => x.TargetId)
                .Column(x => x.DirectoryId)
                .ForeignKey<StorageFileInfo>(x => x.FileId, onDelete: ReferentialAction.SetDefault)
            );
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Destroy(MigrationBuilder builder)
        {
            builder.DropTable<MediaFileInfo>();
            builder.DropTable<StorageFileInfo>();
        }
    }
}