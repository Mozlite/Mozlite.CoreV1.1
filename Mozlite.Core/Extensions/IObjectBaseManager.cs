using System.Threading.Tasks;
using Mozlite.Data;

namespace Mozlite.Extensions
{
    /// <summary>
    /// 对象管理接口。
    /// </summary>
    /// <typeparam name="TModel">实体类型。</typeparam>
    public interface IObjectBaseManager<TModel>
        where TModel : IObjectBase
    {
        /// <summary>
        /// 判断实例是否重复。
        /// </summary>
        /// <param name="model">模型实例。</param>
        /// <returns>返回判断结果。</returns>
        bool IsDulicate(TModel model);

        /// <summary>
        /// 判断实例是否重复。
        /// </summary>
        /// <param name="model">模型实例。</param>
        /// <returns>返回判断结果。</returns>
        Task<bool> IsDulicateAsync(TModel model);

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        DataResult Create(TModel model);

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        Task<DataResult> CreateAsync(TModel model);

        /// <summary>
        /// 更新实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        DataResult Update(TModel model);

        /// <summary>
        /// 更新实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        Task<DataResult> UpdateAsync(TModel model);

        /// <summary>
        /// 更新实例。
        /// </summary>
        /// <param name="id">当前唯一Id。</param>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        DataResult Update(int id, object model);

        /// <summary>
        /// 更新实例。
        /// </summary>
        /// <param name="id">当前唯一Id。</param>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        Task<DataResult> UpdateAsync(int id, object model);

        /// <summary>
        /// 保存实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        DataResult Save(TModel model);

        /// <summary>
        /// 保存实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        Task<DataResult> SaveAsync(TModel model);

        /// <summary>
        /// 分页加载文档列表。
        /// </summary>
        /// <param name="query">文档列表查询实例。</param>
        /// <returns>返回文档列表。</returns>
        TQuery Load<TQuery>(TQuery query) where TQuery : QueryBase<TModel>;

        /// <summary>
        /// 分页加载文档列表。
        /// </summary>
        /// <param name="query">文档列表查询实例。</param>
        /// <returns>返回文档列表。</returns>
        Task<TQuery> LoadAsync<TQuery>(TQuery query) where TQuery : QueryBase<TModel>;

        /// <summary>
        /// 获取模型实例。
        /// </summary>
        /// <param name="id">实体Id。</param>
        /// <returns>返回模型实例对象。</returns>
        TModel Find(int id);

        /// <summary>
        /// 获取模型实例。
        /// </summary>
        /// <param name="id">实体Id。</param>
        /// <returns>返回模型实例对象。</returns>
        Task<TModel> FindAsync(int id);

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="id">Id。</param>
        /// <returns>返回删除结果。</returns>
        DataResult Delete(int id);

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="ids">Id集合。</param>
        /// <returns>返回删除结果。</returns>
        DataResult Delete(int[] ids);

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="ids">Id集合，以“,”分割。</param>
        /// <returns>返回删除结果。</returns>
        DataResult Delete(string ids);

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="id">Id。</param>
        /// <returns>返回删除结果。</returns>
        Task<DataResult> DeleteAsync(int id);

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="ids">Id集合。</param>
        /// <returns>返回删除结果。</returns>
        Task<DataResult> DeleteAsync(int[] ids);

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="ids">Id集合，以“,”分割。</param>
        /// <returns>返回删除结果。</returns>
        Task<DataResult> DeleteAsync(string ids);
    }
}