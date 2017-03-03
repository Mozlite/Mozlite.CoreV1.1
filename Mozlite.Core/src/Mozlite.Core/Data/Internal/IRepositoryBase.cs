using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mozlite.Data.Internal
{
    /// <summary>
    /// 数据库操作接口。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public interface IRepositoryBase<TModel> : IExecutor
    {
        /// <summary>
        /// 获取当前条件下的数量。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回当前数量值。</returns>
        int Count(Expression<Predicate<TModel>> expression);

        /// <summary>
        /// 获取当前条件下的数量。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回当前数量值。</returns>
        Task<int> CountAsync(Expression<Predicate<TModel>> expression, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 新建实例对象。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回是否成功新建实例。</returns>
        bool Create(TModel model);

        /// <summary>
        /// 新建实例对象。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否成功新建实例。</returns>
        Task<bool> CreateAsync(TModel model, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 更新模型实例，此实例必须包含自增长ID或主键实例对象。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回是否更新成功。</returns>
        bool Update(TModel model);

        /// <summary>
        /// 更新模型实例，此实例必须包含自增长ID或主键实例对象。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否更新成功。</returns>
        Task<bool> UpdateAsync(TModel model, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// 更新所有模型实例对象。
        /// </summary>
        /// <param name="statement">更新选项实例。</param>
        /// <returns>返回是否更新成功。</returns>
        bool Update(object statement);

        /// <summary>
        /// 根据条件更新相应的模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="statement">更新选项实例。</param>
        /// <returns>返回是否更新成功。</returns>
        bool Update(Expression<Predicate<TModel>> expression, object statement);

        /// <summary>
        /// 根据条件更新相应的模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="statement">更新选项实例。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否更新成功。</returns>
        Task<bool> UpdateAsync(Expression<Predicate<TModel>> expression, object statement,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 更新所有的模型实例对象。
        /// </summary>
        /// <param name="statement">更新选项实例。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否更新成功。</returns>
        Task<bool> UpdateAsync(object statement, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 根据条件删除模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>判断是否删除成功。</returns>
        bool Delete(Expression<Predicate<TModel>> expression = null);

        /// <summary>
        /// 根据条件删除模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>判断是否删除成功。</returns>
        Task<bool> DeleteAsync(Expression<Predicate<TModel>> expression = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 通过条件表达式获取模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回模型实例对象。</returns>
        TModel Find(Expression<Predicate<TModel>> expression);

        /// <summary>
        /// 通过条件表达式获取模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回模型实例对象。</returns>
        Task<TModel> FindAsync(Expression<Predicate<TModel>> expression,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 通过条件表达式获取模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回模型实例对象。</returns>
        IEnumerable<TModel> Load(Expression<Predicate<TModel>> expression = null);

        /// <summary>
        /// 通过条件表达式获取模型实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回模型实例对象。</returns>
        Task<IEnumerable<TModel>> LoadAsync(Expression<Predicate<TModel>> expression = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 通过条件表达式判断是否存在实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回判断结果。</returns>
        bool Any(Expression<Predicate<TModel>> expression = null);

        /// <summary>
        /// 通过条件表达式判断是否存在实例对象。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回判断结果。</returns>
        Task<bool> AnyAsync(Expression<Predicate<TModel>> expression = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 自增加更新。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">更新列表达式。</param>
        /// <param name="value">每个属性更新的量。</param>
        /// <returns>返回是否更新成功。</returns>
        bool IncreaseBy(Expression<Predicate<TModel>> expression, Expression<Func<TModel, object>> columns, object value);

        /// <summary>
        /// 自增加更新。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">更新列表达式。</param>
        /// <param name="value">每个属性更新的量。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否更新成功。</returns>
        Task<bool> IncreaseByAsync(Expression<Predicate<TModel>> expression, Expression<Func<TModel, object>> columns, object value,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 自减少更新。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">更新列表达式。</param>
        /// <param name="value">每个属性更新的量。</param>
        /// <returns>返回是否更新成功。</returns>
        bool DecreaseBy(Expression<Predicate<TModel>> expression, Expression<Func<TModel, object>> columns, object value);

        /// <summary>
        /// 自减少更新。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="columns">更新列表达式。</param>
        /// <param name="value">每个属性更新的量。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>返回是否更新成功。</returns>
        Task<bool> DecreaseByAsync(Expression<Predicate<TModel>> expression, Expression<Func<TModel, object>> columns, object value,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}