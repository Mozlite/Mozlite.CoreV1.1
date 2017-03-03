using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using Mozlite.Data.Metadata;
using Mozlite.Data.Query;
using Mozlite.Utils;

namespace Mozlite.Data.SqlServer.Query
{
    /// <summary>
    /// SQLServer数据库查询字符串生成器。
    /// </summary>
    public class SqlServerGenerator : SqlGenerator
    {
        /// <summary>
        /// 获取插入数据后自增长的SQL字符串。
        /// </summary>
        /// <returns>返回自增长获取的SQL字符串。</returns>
        protected override string SelectIdentity()
        {
            return "SELECT SCOPE_IDENTITY();";
        }

        /// <summary>
        /// 生成实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回SQL脚本。</returns>
        public override SqlScript Any(IEntityType entity, Expression expression)
        {
            var builder = NewBuilder(entity, Ignore.List, "TOP(1) 1");
            builder.AppendEx(Visit(expression), "WHERE {0}");
            return new SqlScript(builder);
        }

        /// <summary>
        /// 查询脚本。
        /// </summary>
        /// <param name="sql">当前查询实例。</param>
        /// <param name="builder"><see cref="IndentedStringBuilder"/>实例。</param>
        protected override void Query(IQuerySql sql, IndentedStringBuilder builder)
        {
            builder.Append("SELECT ");
            if (sql.IsDistinct)
                builder.Append("DISTINCT ");
            builder.Append(sql.FieldSql).Append(" ");
            builder.Append(sql.FromSql).Append(" ");
            builder.Append(sql.WhereSql).Append(" ");
            builder.Append(sql.OrderBySql).Append(SqlHelper.StatementTerminator);
        }

        /// <summary>
        /// 分页查询脚本。
        /// </summary>
        /// <param name="sql">当前查询实例。</param>
        /// <param name="builder"><see cref="IndentedStringBuilder"/>实例。</param>
        protected override void PageQuery(IQuerySql sql, IndentedStringBuilder builder)
        {
            builder.Append("SELECT ");
            if (sql.IsDistinct)
                builder.Append("DISTINCT ");
            builder.Append(sql.FieldSql).Append(" ");
            builder.Append(sql.FromSql).Append(" ");
            builder.Append(sql.WhereSql).Append(" ");
            builder.Append(sql.OrderBySql).Append(" ");

            var size = sql.Size ?? 20;
            builder.Append("OFFSET ")
                .Append(Math.Max((sql.PageIndex.Value - 1) * size, 0))
                .Append(" ROWS FETCH NEXT ")
                .Append(size)
                .AppendLine(" ROWS ONLY;");

            builder.Append("SELECT COUNT(");
            if (sql.IsDistinct)
                builder.Append("DISTINCT ").Append(sql.Aggregation);
            else
                builder.Append("1");
            builder.Append(")");
            builder.Append(sql.FromSql).Append(" ");
            builder.Append(sql.WhereSql).Append(";");
        }

        /// <summary>
        /// 选项特定数量的记录数的查询脚本。
        /// </summary>
        /// <param name="sql">当前查询实例。</param>
        /// <param name="builder"><see cref="IndentedStringBuilder"/>实例。</param>
        protected override void SizeQuery(IQuerySql sql, IndentedStringBuilder builder)
        {
            builder.Append("SELECT ");
            if (sql.IsDistinct)
                builder.Append("DISTINCT ");
            builder.Append("TOP(").Append(sql.Size).Append(") ");
            builder.Append(sql.FieldSql).Append(" ");
            builder.Append(sql.FromSql).Append(" ");
            builder.Append(sql.WhereSql).Append(" ");
            builder.Append(sql.OrderBySql).Append(SqlHelper.StatementTerminator);
        }

        /// <summary>
        /// 递归的查询脚本。
        /// </summary>
        /// <param name="entityType">当前查询类型实例。</param>
        /// <param name="expression">表达式。</param>
        /// <param name="isParent">父级。</param>
        /// <returns>返回脚本实例。</returns>
        public override SqlScript Recurse(IEntityType entityType, Expression expression, bool isParent = false)
        {
            var table = Model.GetTable(entityType.ClrType);
            var fields = string.Join(",",
                entityType.FindProperties(Ignore.List).Select(p => SqlHelper.DelimitIdentifier(p.Name)));
            var builder = new IndentedStringBuilder();
            builder.Append("WITH _recursive(").Append(fields).Append(")as(");
            builder.Append("SELECT ").Append(fields)
                .Append(" FROM ").Append(table).AppendEx(Visit(expression), "WHERE {0}");
            builder.Append(" UNION ALL (");
            builder.Append("SELECT ").Append(fields).Append(" FROM ");
            builder.Append(table).Append(" a INNER JOIN _recursive b ON ");
            if (isParent)
                builder.Append("a.Id == b.ParentId)");
            else
                builder.Append("a.ParentId == b.Id)");
            builder.Append(") SELECT * FROM _recursive;");
            return new SqlScript(builder);
        }

        /// <summary>
        /// 获取搜索实例，需要将搜索实例进行分词索引。
        /// </summary>
        /// <param name="entityType">实体类型。</param>
        /// <returns>返回脚本实例。</returns>
        public override string SearchIndex(IEntityType entityType)
        {
            return $"SELECT TOP(1)* FROM {Model.GetTable(entityType.ClrType)} WHERE IsSearchIndexed = 0 AND [Status] = 0;";
        }

        /// <summary>
        /// 初始化类<see cref="SqlServerGenerator"/>。
        /// </summary>
        /// <param name="cache">缓存接口。</param>
        /// <param name="sqlHelper">SQL辅助接口。</param>
        /// <param name="visitorFactory">表达式工厂接口。</param>
        /// <param name="model">模型接口。</param>
        public SqlServerGenerator(IMemoryCache cache, ISqlHelper sqlHelper, IExpressionVisitorFactory visitorFactory, IModel model)
            : base(cache, sqlHelper, visitorFactory, model)
        {
        }
    }
}