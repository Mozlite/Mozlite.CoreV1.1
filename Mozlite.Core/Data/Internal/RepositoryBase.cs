using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mozlite.Data.Metadata;
using Mozlite.Data.Query;

namespace Mozlite.Data.Internal
{
    /// <summary>
    /// 实体数据库操作基类。
    /// </summary>
    /// <typeparam name="TModel">实体模型。</typeparam>
    public abstract class RepositoryBase<TModel> : IRepositoryBase<TModel>
    {
        /// <summary>
        /// 日志接口。
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// SQL辅助接口。
        /// </summary>
        protected ISqlHelper SqlHelper { get; }

        /// <summary>
        /// 脚本生成接口。
        /// </summary>
        protected ISqlGenerator SqlGenerator { get; }
        private readonly IExecutor _executor;
        /// <summary>
        /// 当前模型的实体类型。
        /// </summary>
        protected IEntityType Entity { get; }

        /// <summary>
        /// 初始化类<see cref="RepositoryBase{TModel}"/>。
        /// </summary>
        /// <param name="executor">数据库执行接口。</param>
        /// <param name="logger">日志接口。</param>
        /// <param name="entityType">模型实例接口。</param>
        /// <param name="sqlHelper">SQL辅助接口。</param>
        /// <param name="sqlGenerator">脚本生成器。</param>
        protected RepositoryBase(IExecutor executor, ILogger logger, IEntityType entityType, ISqlHelper sqlHelper, ISqlGenerator sqlGenerator)
        {
            Logger = logger;
            SqlHelper = sqlHelper;
            SqlGenerator = sqlGenerator;
            _executor = executor;
            Entity = entityType;
        }

        /// <summary>
        /// 执行没有返回值的查询实例对象。
        /// </summary>
        /// <param name="commandText">SQL字符串。</param>
        /// <param name="parameters">参数实例对象。</param>
        /// <param name="commandType">命令类型。</param>
        /// <returns>返回是否有执行影响到数据行。</returns>
        public bool ExecuteNonQuery(string commandText, object parameters = null,
                CommandType commandType = CommandType.Text)
            => _executor.ExecuteNonQuery(commandText, parameters, commandType);

        /// <summary>
        /// 查询实例对象。
        /// </summary>
        /// <param name="commandText">SQL字符串。</param>
        /// <param name="parameters">参数实例对象。</param>
        /// <param name="commandType">命令类型。</param>
        /// <returns>返回数据库读取实例接口。</returns>
        public DbDataReader ExecuteReader(string commandText, object parameters = null, CommandType commandType = CommandType.Text)
            => _executor.ExecuteReader(commandText, parameters, commandType);

        /// <summary>
        /// 查询数据库聚合值。
        /// </summary>
        /// <param name="commandText">SQL字符串。</param>
        /// <param name="parameters">参数实例对象。</param>
        /// <param name="commandType">命令类型。</param>
        /// <returns>返回聚合值实例对象。</returns>
        public object ExecuteScalar(string commandText, object parameters = null, CommandType commandType = CommandType.Text)
            => _executor.ExecuteScalar(commandText, parameters, commandType);

        /// <summary>
        /// 执行SQL语句。
        /// </summary>
        /// <param name="commandText">SQL字符串。</param>
        /// <param name="parameters">参数匿名类型。</param>
        /// <param name="commandType">SQL类型。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回影响的行数。</returns>
        public Task<bool> ExecuteNonQueryAsync(string commandText, object parameters = null, CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = new CancellationToken())
            => _executor.ExecuteNonQueryAsync(commandText, parameters, commandType, cancellationToken);

        /// <summary>
        /// 执行SQL语句。
        /// </summary>
        /// <param name="commandText">SQL字符串。</param>
        /// <param name="commandType">SQL类型。</param>
        /// <param name="parameters">参数匿名类型。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回数据库读取器实例对象。</returns>
        public Task<DbDataReader> ExecuteReaderAsync(string commandText, object parameters = null, CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = new CancellationToken())
            => _executor.ExecuteReaderAsync(commandText, parameters, commandType, cancellationToken);

        /// <summary>
        /// 执行SQL语句。
        /// </summary>
        /// <param name="commandText">SQL字符串。</param>
        /// <param name="commandType">SQL类型。</param>
        /// <param name="parameters">参数匿名类型。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回单一结果实例对象。</returns>
        public Task<object> ExecuteScalarAsync(string commandText, object parameters = null, CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = new CancellationToken())
            => _executor.ExecuteScalarAsync(commandText, parameters, commandType, cancellationToken);

        /// <summary>
        /// 获取当前条件下的数量。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回当前数量值。</returns>
        public int Count(Expression<Predicate<TModel>> expression)
        {
            var sql = SqlGenerator.Scalar(Entity, "COUNT", null, expression);
            return Convert.ToInt32(ExecuteScalar(sql.ToString()));
        }

        /// <summary>
        /// 获取当前条件下的数量。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回当前数量值。</returns>
        public async Task<int> CountAsync(Expression<Predicate<TModel>> expression, CancellationToken cancellationToken = new CancellationToken())
        {
            var sql = SqlGenerator.Scalar(Entity, "COUNT", null, expression);
            return Convert.ToInt32(await ExecuteScalarAsync(sql.ToString(), cancellationToken: cancellationToken));
        }

        /// <summary>
        /// 新建实例对象。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回是否成功新建实例。</returns>
        public bool Create(TModel model)
        {
            var sql = SqlGenerator.Create(Entity);
            if (Entity.Identity != null)
            {
                var id = ExecuteScalar(sql.ToString(), sql.CreateParameters(model));
                if (id != null)
                {
                    Entity.Identity.Set(model, Convert.ToInt32(id));
                    return true;
                }
                return false;
            }
            return ExecuteNonQuery(sql.ToString(), sql.CreateParameters(model));
        }

        /// <summary>
        /// 新建实例对象。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否成功新建实例。</returns>
        public async Task<bool> CreateAsync(TModel model, CancellationToken cancellationToken = new CancellationToken())
        {
            var sql = SqlGenerator.Create(Entity);
            if (Entity.Identity != null)
            {
                var id = await ExecuteScalarAsync(sql.ToString(), sql.CreateParameters(model), cancellationToken: cancellationToken);
                if (id != null)
                {
                    if (Entity.Identity.ClrType == typeof(int))
                        Entity.Identity.Set(model, Convert.ToInt32(id));
                    else if (Entity.Identity.ClrType == typeof(long))
                        Entity.Identity.Set(model, Convert.ToInt64(id));
                    else if (Entity.Identity.ClrType == typeof(short))
                        Entity.Identity.Set(model, Convert.ToInt16(id));
                    return true;
                }
                return false;
            }
            return await ExecuteNonQueryAsync(sql.ToString(), sql.CreateParameters(model), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 更新模型实例，此实例必须包含自增长ID或主键实例对象。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回是否更新成功。</returns>
        public bool Update(TModel model)
        {
            var sql = SqlGenerator.Update(Entity);
            return ExecuteNonQuery(sql.ToString(), sql.CreateParameters(model));
        }

        /// <summary>
        /// 更新模型实例，此实例必须包含自增长ID或主键实例对象。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否更新成功。</returns>
        public async Task<bool> UpdateAsync(TModel model, CancellationToken cancellationToken = new CancellationToken())
        {
            var sql = SqlGenerator.Update(Entity);
            return await ExecuteNonQueryAsync(sql.ToString(), sql.CreateParameters(model), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 根据条件更新相应的模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="statement">更新选项实例。</param>
        /// <returns>返回是否更新成功。</returns>
        public bool Update(Expression<Predicate<TModel>> expression, object statement)
        {
            var sql = SqlGenerator.Update(Entity, statement, expression);
            return ExecuteNonQuery(sql.ToString(), sql.Parameters);
        }

        /// <summary>
        /// 更新所有模型实例对象。
        /// </summary>
        /// <param name="statement">更新选项实例。</param>
        /// <returns>返回是否更新成功。</returns>
        public bool Update(object statement)
            => Update(null, statement);

        /// <summary>
        /// 根据条件更新相应的模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="statement">更新选项实例。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否更新成功。</returns>
        public async Task<bool> UpdateAsync(Expression<Predicate<TModel>> expression, object statement, CancellationToken cancellationToken = new CancellationToken())
        {
            var sql = SqlGenerator.Update(Entity, statement, expression);
            return await ExecuteNonQueryAsync(sql.ToString(), sql.Parameters, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 更新所有的模型实例对象。
        /// </summary>
        /// <param name="statement">更新选项实例。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否更新成功。</returns>
        public Task<bool> UpdateAsync(object statement, CancellationToken cancellationToken = new CancellationToken())
            => UpdateAsync(null, statement, cancellationToken);

        /// <summary>
        /// 根据条件删除模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>判断是否删除成功。</returns>
        public bool Delete(Expression<Predicate<TModel>> expression = null)
        {
            var sql = SqlGenerator.Delete(Entity, expression);
            return ExecuteNonQuery(sql.ToString());
        }

        /// <summary>
        /// 根据条件删除模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>判断是否删除成功。</returns>
        public Task<bool> DeleteAsync(Expression<Predicate<TModel>> expression = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var sql = SqlGenerator.Delete(Entity, expression);
            return ExecuteNonQueryAsync(sql.ToString(), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 通过条件表达式获取模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回模型实例对象。</returns>
        public TModel Find([NotNull]Expression<Predicate<TModel>> expression)
        {
            Check.NotNull(expression, nameof(expression));
            var sql = SqlGenerator.List(Entity, expression);
            return ReadSql(sql);
        }

        /// <summary>
        /// 读取模型实例。
        /// </summary>
        /// <param name="sql">SQL语句。</param>
        /// <returns>返回模型实例。</returns>
        protected TModel ReadSql(SqlScript sql)
        {
            using (var reader = ExecuteReader(sql.ToString(), sql.Parameters))
            {
                if (reader.Read())
                    return Entity.Read<TModel>(reader);
            }
            return default(TModel);
        }


        /// <summary>
        /// 读取模型实例。
        /// </summary>
        /// <param name="sql">SQL语句。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回模型实例。</returns>
        protected async Task<TModel> ReadSqlAsync(SqlScript sql, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var reader = await ExecuteReaderAsync(sql.ToString(), sql.Parameters, cancellationToken: cancellationToken))
            {
                if (await reader.ReadAsync(cancellationToken))
                    return Entity.Read<TModel>(reader);
            }
            return default(TModel);
        }

        /// <summary>
        /// 通过条件表达式获取模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回模型实例对象。</returns>
        public async Task<TModel> FindAsync(Expression<Predicate<TModel>> expression, CancellationToken cancellationToken = new CancellationToken())
        {
            Check.NotNull(expression, nameof(expression));
            var sql = SqlGenerator.List(Entity, expression);
            return await ReadSqlAsync(sql, cancellationToken);
        }

        /// <summary>
        /// 通过条件表达式获取模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回模型实例对象。</returns>
        public IEnumerable<TModel> Load(Expression<Predicate<TModel>> expression = null)
        {
            var sql = SqlGenerator.List(Entity, expression);
            return LoadSql(sql);
        }

        /// <summary>
        /// 读取模型实例列表。
        /// </summary>
        /// <param name="sql">SQL语句。</param>
        /// <returns>返回模型实例列表。</returns>
        protected IEnumerable<TModel> LoadSql(SqlScript sql)
        {
            var models = new List<TModel>();
            using (var reader = ExecuteReader(sql.ToString(), sql.Parameters))
            {
                while (reader.Read())
                    models.Add(Entity.Read<TModel>(reader));
            }
            return models;
        }


        /// <summary>
        /// 读取模型实例列表。
        /// </summary>
        /// <param name="sql">SQL语句。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回模型实例列表。</returns>
        protected async Task<IEnumerable<TModel>> LoadSqlAsync(SqlScript sql, CancellationToken cancellationToken = new CancellationToken())
        {
            var models = new List<TModel>();
            using (var reader = await ExecuteReaderAsync(sql.ToString(), sql.Parameters, cancellationToken: cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                    models.Add(Entity.Read<TModel>(reader));
            }
            return models;
        }

        /// <summary>
        /// 通过条件表达式获取模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回模型实例对象。</returns>
        public async Task<IEnumerable<TModel>> LoadAsync(Expression<Predicate<TModel>> expression = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var sql = SqlGenerator.List(Entity, expression);
            return await LoadSqlAsync(sql, cancellationToken);
        }

        /// <summary>
        /// 通过条件表达式判断是否存在实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回判断结果。</returns>
        public bool Any(Expression<Predicate<TModel>> expression = null)
        {
            var sql = SqlGenerator.Any(Entity, expression);
            return ExecuteScalar(sql.ToString()) != null;
        }

        /// <summary>
        /// 通过条件表达式判断是否存在实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回判断结果。</returns>
        public async Task<bool> AnyAsync(Expression<Predicate<TModel>> expression = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var sql = SqlGenerator.Any(Entity, expression);
            return await ExecuteScalarAsync(sql.ToString(), cancellationToken: cancellationToken) != null;
        }

        /// <summary>
        /// 自增加更新。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">更新列表达式。</param>
        /// <param name="value">每个属性更新的量。</param>
        /// <returns>返回是否更新成功。</returns>
        public virtual bool IncreaseBy(Expression<Predicate<TModel>> expression, Expression<Func<TModel, object>> columns, object value)
        {
            var columnNames = columns.GetPropertyAccessList().Select(p => p.Name).ToArray();
            var sql = SqlGenerator.IncreaseBy(Entity, expression, columnNames);
            return ExecuteNonQuery(sql.ToString(), new { Value = value });
        }

        /// <summary>
        /// 自增加更新。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">更新列表达式。</param>
        /// <param name="value">每个属性更新的量。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否更新成功。</returns>
        public virtual Task<bool> IncreaseByAsync(Expression<Predicate<TModel>> expression, Expression<Func<TModel, object>> columns, object value,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var columnNames = columns.GetPropertyAccessList().Select(p => p.Name).ToArray();
            var sql = SqlGenerator.IncreaseBy(Entity, expression, columnNames);
            return ExecuteNonQueryAsync(sql.ToString(), new { Value = value }, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 自减少更新。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">更新列表达式。</param>
        /// <param name="value">每个属性更新的量。</param>
        /// <returns>返回是否更新成功。</returns>
        public virtual bool DecreaseBy(Expression<Predicate<TModel>> expression, Expression<Func<TModel, object>> columns, object value)
        {
            var columnNames = columns.GetPropertyAccessList().Select(p => p.Name).ToArray();
            var sql = SqlGenerator.DecreaseBy(Entity, expression, columnNames);
            return ExecuteNonQuery(sql.ToString(), new { Value = value });
        }

        /// <summary>
        /// 自减少更新。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">更新列表达式。</param>
        /// <param name="value">每个属性更新的量。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否更新成功。</returns>
        public virtual Task<bool> DecreaseByAsync(Expression<Predicate<TModel>> expression, Expression<Func<TModel, object>> columns, object value,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var columnNames = columns.GetPropertyAccessList().Select(p => p.Name).ToArray();
            var sql = SqlGenerator.DecreaseBy(Entity, expression, columnNames);
            return ExecuteNonQueryAsync(sql.ToString(), new { Value = value }, cancellationToken: cancellationToken);
        }
    }
}