using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using Mozlite.Core;
using Mozlite.Data.Metadata;
using Mozlite.Utils;
using System.Reflection;

namespace Mozlite.Data.Query
{
    /// <summary>
    /// SQL生成接口。
    /// </summary>
    public interface ISqlGenerator
    {
        /// <summary>
        /// 生成新建实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <returns>返回SQL脚本。</returns>
        SqlScript Create(IEntityType entity);

        /// <summary>
        /// 生成更新实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <returns>返回SQL脚本。</returns>
        SqlScript Update(IEntityType entity);

        /// <summary>
        /// 生成更新实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <param name="statement">匿名对象。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回SQL脚本。</returns>
        SqlScript Update(IEntityType entity, object statement, Expression expression);

        /// <summary>
        /// 生成实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回SQL脚本。</returns>
        SqlScript List(IEntityType entity, Expression expression);

        /// <summary>
        /// 生成实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回SQL脚本。</returns>
        SqlScript Delete(IEntityType entity, Expression expression);

        /// <summary>
        /// 生成实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回SQL脚本。</returns>
        SqlScript Any(IEntityType entity, Expression expression);

        /// <summary>
        /// 生成实体类型的SQL脚本。
        /// </summary>
        /// <param name="sql">SQL查询实例。</param>
        /// <returns>返回SQL脚本。</returns>
        SqlScript Query(IQuerySql sql);

        /// <summary>
        /// 递归的查询脚本。
        /// </summary>
        /// <param name="entityType">当前查询类型实例。</param>
        /// <param name="expression">表达式。</param>
        /// <param name="isParent">父级。</param>
        /// <returns>返回脚本实例。</returns>
        SqlScript Recurse(IEntityType entityType, Expression expression, bool isParent = false);

        /// <summary>
        /// 将对象列的是否索引列设为索引。
        /// </summary>
        /// <param name="entityType">实体类型。</param>
        /// <returns>返回脚本实例。</returns>
        string SetSearchIndexed(IEntityType entityType);

        /// <summary>
        /// 获取搜索实例，需要将搜索实例进行分词索引。
        /// </summary>
        /// <param name="entityType">实体类型。</param>
        /// <returns>返回脚本实例。</returns>
        string SearchIndex(IEntityType entityType);

        /// <summary>
        /// 自增加更新，参数名称为Value。
        /// </summary>
        /// <param name="entityType">实体类型。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">自增加的列名称列表。</param>
        /// <returns>返回脚本实例。</returns>
        SqlScript IncreaseBy(IEntityType entityType, Expression expression, string[] columns);

        /// <summary>
        /// 自减少更新，参数名称为Value。
        /// </summary>
        /// <param name="entityType">实体类型。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">自增加的列名称列表。</param>
        /// <returns>返回脚本实例。</returns>
        SqlScript DecreaseBy(IEntityType entityType, Expression expression, string[] columns);

        /// <summary>
        /// 生成实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <param name="innerExpression">聚合表达式。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="funcName">方法名称。</param>
        /// <returns>返回SQL脚本。</returns>
        SqlScript Scalar(IEntityType entity, string funcName, Expression innerExpression, Expression expression);
    }

    /// <summary>
    /// SQL脚本生成基类。
    /// </summary>
    public abstract class SqlGenerator : ISqlGenerator
    {
        /// <summary>
        /// SQL辅助接口。
        /// </summary>
        protected ISqlHelper SqlHelper { get; }
        /// <summary>
        /// 模型接口。
        /// </summary>
        protected IModel Model { get; }
        private readonly IMemoryCache _cache;
        private readonly IExpressionVisitorFactory _visitorFactory;

        /// <summary>
        /// 初始化类<see cref="SqlGenerator"/>。
        /// </summary>
        /// <param name="cache">缓存接口。</param>
        /// <param name="sqlHelper">SQL辅助接口。</param>
        /// <param name="visitorFactory">表达式工厂接口。</param>
        /// <param name="model">模型接口。</param>
        protected SqlGenerator(IMemoryCache cache, ISqlHelper sqlHelper, IExpressionVisitorFactory visitorFactory, IModel model)
        {
            SqlHelper = sqlHelper;
            Model = model;
            _cache = cache;
            _visitorFactory = visitorFactory;
        }

        /// <summary>
        /// 添加表格。
        /// </summary>
        /// <param name="entity">模型实例。</param>
        /// <param name="ignore">操作方法。</param>
        /// <param name="selections">选项字符串。</param>
        /// <returns>返回字符串构建实例。</returns>
        protected IndentedStringBuilder NewBuilder(IEntityType entity, Ignore ignore, string selections = null)
        {
            var builder = new IndentedStringBuilder();
            Func<string, string, IndentedStringBuilder> append = (prefix, suffix) =>
            {
                builder.Append(prefix).Append(" ");
                builder.Append(Model.GetTable(entity.ClrType));
                builder.Append(suffix);
                return builder;
            };
            switch (ignore)
            {
                case Ignore.Insert:
                    return append("INSERT INTO", null);
                case Ignore.List:
                    selections = selections ?? "*";
                    return append($"SELECT {selections} FROM", " ");
                case Ignore.Update:
                    return append("UPDATE", " SET ");
            }
            return append("DELETE FROM", " ");
        }

        /// <summary>
        /// 生成新建实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <returns>返回SQL脚本。</returns>
        public virtual SqlScript Create(IEntityType entity)
        {
            return Cached(entity, nameof(Create), () =>
            {
                var builder = NewBuilder(entity, Ignore.Insert);
                var names = entity.FindProperties(Ignore.Insert)
                    .Select(name => name.Name);
                builder.Append("(")
                       .JoinAppend(names.Select(SqlHelper.DelimitIdentifier))
                       .AppendLine(")");
                builder.Append("VALUES(")
                    .JoinAppend(names.Select(SqlHelper.Parameterized))
                    .Append(")")
                    .Append(SqlHelper.StatementTerminator);
                if (entity.Identity != null)
                    builder.Append(SelectIdentity());
                return new SqlScript(builder, names, entity);
            });
        }

        /// <summary>
        /// 缓存脚本。
        /// </summary>
        /// <param name="entityType">当前实体。</param>
        /// <param name="name">方法名称。</param>
        /// <param name="func">获取脚本的方法。</param>
        /// <returns>返回SQL脚本。</returns>
        protected SqlScript Cached(IEntityType entityType, string name, Func<SqlScript> func)
        {
            return _cache.GetOrCreate($"{entityType.ClrType.DisplayName()}[{name}]", ctx =>
            {
                ctx.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return func();
            });
        }

        /// <summary>
        /// 生成更新实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <returns>返回SQL脚本。</returns>
        public virtual SqlScript Update(IEntityType entity)
        {
            return Cached(entity, nameof(Update), () =>
            {
                var builder = NewBuilder(entity, Ignore.Update);
                var names = entity.FindProperties(Ignore.Update)
                    .Select(name => name.Name);
                var list = new List<string>();
                foreach (var name in names)
                {
                    list.Add($"{SqlHelper.DelimitIdentifier(name)}={SqlHelper.Parameterized(name)}");
                }
                builder.JoinAppend(list).AppendLine();
                if (entity.PrimaryKey != null)
                {
                    builder.Append("WHERE ")
                        .JoinAppend(
                            entity.PrimaryKey.Properties.Select(
                                name => $"{SqlHelper.DelimitIdentifier(name.Name)}={SqlHelper.Parameterized(name.Name)}"))
                                .Append(SqlHelper.StatementTerminator);
                    names = names.Concat(entity.PrimaryKey.Properties.Select(p => p.Name));
                }
                return new SqlScript(builder, names.Distinct(StringComparer.OrdinalIgnoreCase), entity);
            });
        }

        /// <summary>
        /// 获取自增长的SQL脚本字符串。
        /// </summary>
        /// <returns>自增长的SQL脚本字符串。</returns>
        protected abstract string SelectIdentity();

        /// <summary>
        /// 生成更新实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <param name="statement">匿名对象。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回SQL脚本。</returns>
        public SqlScript Update(IEntityType entity, object statement, Expression expression)
        {
            var builder = NewBuilder(entity, Ignore.Update);
            var list = new List<string>();
            var parameters = CreateParameters(statement, (name, value) =>
            {
                list.Add($"{SqlHelper.DelimitIdentifier(name)}={SqlHelper.Parameterized(name)}");
                return value;
            });
            builder.JoinAppend(list).AppendLine();
            builder.AppendEx(Visit(expression), "WHERE {0}");
            return new SqlScript(builder, parameters);
        }

        /// <summary>
        /// 生成实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回SQL脚本。</returns>
        public SqlScript List(IEntityType entity, Expression expression)
        {
            var builder = NewBuilder(entity, Ignore.List);
            builder.AppendEx(Visit(expression), "WHERE {0}");
            return new SqlScript(builder);
        }

        /// <summary>
        /// 生成实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回SQL脚本。</returns>
        public SqlScript Delete(IEntityType entity, Expression expression)
        {
            var builder = NewBuilder(entity, Ignore.None);
            builder.AppendEx(Visit(expression), "WHERE {0}");
            return new SqlScript(builder);
        }

        /// <summary>
        /// 生成实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回SQL脚本。</returns>
        public abstract SqlScript Any(IEntityType entity, Expression expression);

        /// <summary>
        /// 生成实体类型的SQL脚本。
        /// </summary>
        /// <param name="sql">SQL查询实例。</param>
        /// <returns>返回SQL脚本。</returns>
        public SqlScript Query(IQuerySql sql)
        {
            var builder = new IndentedStringBuilder();
            if (sql.PageIndex != null)
                PageQuery(sql, builder);
            else if (sql.Size != null)
                SizeQuery(sql, builder);
            else
                Query(sql, builder);
            return new SqlScript(builder);
        }

        /// <summary>
        /// 查询脚本。
        /// </summary>
        /// <param name="sql">当前查询实例。</param>
        /// <param name="builder"><see cref="IndentedStringBuilder"/>实例。</param>
        protected abstract void Query(IQuerySql sql, IndentedStringBuilder builder);

        /// <summary>
        /// 分页查询脚本。
        /// </summary>
        /// <param name="sql">当前查询实例。</param>
        /// <param name="builder"><see cref="IndentedStringBuilder"/>实例。</param>
        protected abstract void PageQuery(IQuerySql sql, IndentedStringBuilder builder);

        /// <summary>
        /// 选项特定数量的记录数的查询脚本。
        /// </summary>
        /// <param name="sql">当前查询实例。</param>
        /// <param name="builder"><see cref="IndentedStringBuilder"/>实例。</param>
        protected abstract void SizeQuery(IQuerySql sql, IndentedStringBuilder builder);

        /// <summary>
        /// 递归的查询脚本。
        /// </summary>
        /// <param name="entityType">当前查询类型实例。</param>
        /// <param name="expression">表达式。</param>
        /// <param name="isParent">父级。</param>
        /// <returns>返回脚本实例。</returns>
        public abstract SqlScript Recurse(IEntityType entityType, Expression expression, bool isParent = false);

        /// <summary>
        /// 将对象列的是否索引列设为索引。
        /// </summary>
        /// <param name="entityType">实体类型。</param>
        /// <returns>返回脚本实例。</returns>
        public virtual string SetSearchIndexed(IEntityType entityType)
        {
            var builder = NewBuilder(entityType, Ignore.Update);
            builder.Append("IsSearchIndexed = 1 WHERE ")
                .Append(SqlHelper.DelimitIdentifier("Id"))
                .Append("=")
                .Append(SqlHelper.Parameterized("Id"))
                .Append(SqlHelper.StatementTerminator);
            return builder.ToString();
        }

        /// <summary>
        /// 获取搜索实例，需要将搜索实例进行分词索引。
        /// </summary>
        /// <param name="entityType">实体类型。</param>
        /// <returns>返回脚本实例。</returns>
        public abstract string SearchIndex(IEntityType entityType);

        /// <summary>
        /// 自增加更新，参数名称为Value。
        /// </summary>
        /// <param name="entityType">实体类型。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">自增加的列名称列表。</param>
        /// <returns>返回脚本实例。</returns>
        public virtual SqlScript IncreaseBy(IEntityType entityType, Expression expression, string[] columns)
        {
            var builder = NewBuilder(entityType, Ignore.Update);
            var isFirst = true;
            foreach (var column in columns)
            {
                if (isFirst)
                    isFirst = false;
                else
                    builder.Append(", ");
                builder.Append(SqlHelper.DelimitIdentifier(column))
                    .Append("=").Append(SqlHelper.DelimitIdentifier(column))
                    .Append("+").Append(SqlHelper.Parameterized("Value"));
            }
            builder.AppendEx(Visit(expression), " WHERE {0}").Append(SqlHelper.StatementTerminator);
            return new SqlScript(builder);
        }

        /// <summary>
        /// 自减少更新，参数名称为Value。
        /// </summary>
        /// <param name="entityType">实体类型。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">自增加的列名称列表。</param>
        /// <returns>返回脚本实例。</returns>
        public SqlScript DecreaseBy(IEntityType entityType, Expression expression, string[] columns)
        {
            var builder = NewBuilder(entityType, Ignore.Update);
            var isFirst = true;
            foreach (var column in columns)
            {
                if (isFirst)
                    isFirst = false;
                else
                    builder.Append(", ");
                builder.Append(SqlHelper.DelimitIdentifier(column))
                    .Append("=").Append(SqlHelper.DelimitIdentifier(column))
                    .Append("-").Append(SqlHelper.Parameterized("Value"));
            }
            builder.AppendEx(Visit(expression), " WHERE {0}").Append(SqlHelper.StatementTerminator);
            return new SqlScript(builder);
        }

        /// <summary>
        /// 生成实体类型的SQL脚本。
        /// </summary>
        /// <param name="entity">实体类型实例。</param>
        /// <param name="innerExpression">聚合表达式。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="funcName">方法名称。</param>
        /// <returns>返回SQL脚本。</returns>
        public SqlScript Scalar(IEntityType entity, string funcName, Expression innerExpression, Expression expression)
        {
            var builder = NewBuilder(entity, Ignore.List, $"{funcName}({Visit(innerExpression) ?? "1"})");
            builder.AppendEx(Visit(expression), " WHERE {0}").Append(SqlHelper.StatementTerminator);
            return new SqlScript(builder);
        }

        /// <summary>
        /// 解析表达式。
        /// </summary>
        /// <param name="expression">表达式实例。</param>
        /// <returns>返回解析的表达式字符串。</returns>
        protected virtual string Visit(Expression expression)
        {
            if (expression == null)
                return null;
            var visitor = _visitorFactory.Create();
            visitor.Visit(expression);
            return visitor.ToString();
        }

        /// <summary>
        /// 将匿名对象转换为参数列表。
        /// </summary>
        /// <param name="statement">匿名对象。</param>
        /// <param name="each">执行每一项。</param>
        /// <returns>返回参数列表。</returns>
        protected virtual IDictionary<string, object> CreateParameters(object statement, Func<string, object, object> each)
        {
            var parameters = new Dictionary<string, object>();
            foreach (var property in statement.GetType().GetRuntimeProperties())
            {
                var value = property.GetValue(statement);
                value = each(property.Name, value);
                parameters.Add(property.Name, value);
            }
            return parameters;
        }
    }
}