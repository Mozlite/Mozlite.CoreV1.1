using System.Text;
using Mozlite.Data.Metadata;
using Mozlite.Data.Migrations;
using Mozlite.Data.Migrations.Builders;
using Mozlite.Data.Migrations.Models;

namespace Mozlite.Data.SqlServer.Migrations
{
    /// <summary>
    /// SQLServer数据库迁移操作脚本实现类。
    /// </summary>
    public class SqlServerMigrationRepository : MigrationRepository
    {
        /// <summary>
        /// 判断是否存在的脚本。
        /// </summary>
        protected override string ExistsSql
        {
            get
            {
                var builder = new StringBuilder();

                builder.Append("SELECT OBJECT_ID(N'");

                if (Table.Schema != null)
                {
                    builder
                        .Append(SqlHelper.EscapeIdentifier(Table.Schema))
                        .Append(".");
                }

                builder
                    .Append(SqlHelper.EscapeIdentifier(Table.Name))
                    .Append("');");

                return builder.ToString();
            }
        }

        /// <summary>
        /// 创建表格语句。
        /// </summary>
        protected override string CreateSql
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append("CREATE TABLE ").Append(Table).AppendLine("(");
                builder.AppendLine("    [Id]      NVARCHAR (256) NOT NULL,");
                builder.AppendLine("    [Version] INT            DEFAULT ((0)) NOT NULL,");
                builder.AppendLine($"    CONSTRAINT [{OperationHelper.GetName(NameType.PrimaryKey, Table)}] PRIMARY KEY CLUSTERED ([Id] ASC)");
                builder.AppendLine(");");
                return builder.ToString();
            }
        }

        /// <summary>
        /// 初始化类<see cref="MigrationRepository"/>。
        /// </summary>
        /// <param name="db">数据库操作实例。</param>
        /// <param name="model">模型接口。</param>
        /// <param name="sqlHelper">SQL辅助接口。</param>
        /// <param name="sqlGenerator">SQL迁移脚本生成接口。</param>
        public SqlServerMigrationRepository(IRepository<Migration> db, IModel model, ISqlHelper sqlHelper, IMigrationsSqlGenerator sqlGenerator) 
            : base(db, model, sqlHelper, sqlGenerator)
        {
        }
    }
}