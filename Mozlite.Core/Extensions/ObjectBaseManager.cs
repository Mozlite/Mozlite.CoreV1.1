using System.Threading.Tasks;
using Mozlite.Data;

namespace Mozlite.Extensions
{
    /// <summary>
    /// 对象管理实现基类。
    /// </summary>
    /// <typeparam name="TModel">实体类型。</typeparam>
    public abstract class ObjectBaseManager<TModel> : IObjectBaseManager<TModel> where TModel : IObjectBase
    {
        /// <summary>
        /// 数据库操作接口。
        /// </summary>
        protected readonly IRepository<TModel> Database;
        /// <summary>
        /// 初始化类<see cref="ObjectManager{TModel}"/>。
        /// </summary>
        /// <param name="repository">数据库操作接口。</param>
        protected ObjectBaseManager(IRepository<TModel> repository)
        {
            Database = repository;
        }

        /// <summary>
        /// 判断实例是否重复。
        /// </summary>
        /// <param name="model">模型实例。</param>
        /// <returns>返回判断结果。</returns>
        public virtual bool IsDulicate(TModel model)
        {
            return false;
        }

        /// <summary>
        /// 判断实例是否重复。
        /// </summary>
        /// <param name="model">模型实例。</param>
        /// <returns>返回判断结果。</returns>
        public virtual Task<bool> IsDulicateAsync(TModel model)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// 保存实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        public virtual DataResult Save(TModel model)
        {
            if (IsDulicate(model))
                return DataAction.Duplicate;
            if (model.Id > 0)
                return DataResult.FromResult(Database.Update(model), DataAction.Updated);
            return DataResult.FromResult(Database.Create(model), DataAction.Created);
        }

        /// <summary>
        /// 保存实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        public virtual async Task<DataResult> SaveAsync(TModel model)
        {
            if (await IsDulicateAsync(model))
                return DataAction.Duplicate;
            if (model.Id > 0)
                return DataResult.FromResult(await Database.UpdateAsync(model), DataAction.Updated);
            return DataResult.FromResult(await Database.CreateAsync(model), DataAction.Created);
        }

        /// <summary>
        /// 分页加载文档列表。
        /// </summary>
        /// <param name="query">文档列表查询实例。</param>
        /// <returns>返回文档列表。</returns>
        public virtual TQuery Load<TQuery>(TQuery query) where TQuery : QueryBase<TModel>
        {
            return Database.Load(query);
        }

        /// <summary>
        /// 分页加载文档列表。
        /// </summary>
        /// <param name="query">文档列表查询实例。</param>
        /// <returns>返回文档列表。</returns>
        public virtual Task<TQuery> LoadAsync<TQuery>(TQuery query) where TQuery : QueryBase<TModel>
        {
            return Database.LoadAsync(query);
        }

        /// <summary>
        /// 获取模型实例。
        /// </summary>
        /// <param name="id">实体Id。</param>
        /// <returns>返回模型实例对象。</returns>
        public virtual TModel Find(int id)
        {
            return Database.Find(x => x.Id == id);
        }

        /// <summary>
        /// 获取模型实例。
        /// </summary>
        /// <param name="id">实体Id。</param>
        /// <returns>返回模型实例对象。</returns>
        public virtual async Task<TModel> FindAsync(int id)
        {
            return await Database.FindAsync(x => x.Id == id);
        }

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="id">Id。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(int id)
        {
            return DataResult.FromResult(Database.Delete(x => x.Id == id), DataAction.Deleted);
        }

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="ids">Id集合。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(int[] ids)
        {
            return DataResult.FromResult(Database.Delete(x => x.Id.Included(ids)), DataAction.Deleted);
        }

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="ids">Id集合，以“,”分割。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(string ids)
        {
            return Delete(ids.SplitToInt32());
        }

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="id">Id。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<DataResult> DeleteAsync(int id)
        {
            return DataResult.FromResult(await Database.DeleteAsync(x => x.Id == id), DataAction.Deleted);
        }

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="ids">Id集合。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<DataResult> DeleteAsync(int[] ids)
        {
            return DataResult.FromResult(await Database.DeleteAsync(x => x.Id.Included(ids)), DataAction.Deleted);
        }

        /// <summary>
        /// 删除实例对象。
        /// </summary>
        /// <param name="ids">Id集合，以“,”分割。</param>
        /// <returns>返回删除结果。</returns>
        public virtual Task<DataResult> DeleteAsync(string ids)
        {
            return DeleteAsync(ids.SplitToInt32());
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        public virtual DataResult Create(TModel model)
        {
            if (IsDulicate(model))
                return DataAction.Duplicate;
            return DataResult.FromResult(Database.Create(model), DataAction.Created);
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        public virtual async Task<DataResult> CreateAsync(TModel model)
        {
            if (await IsDulicateAsync(model))
                return DataAction.Duplicate;
            return DataResult.FromResult(await Database.CreateAsync(model), DataAction.Created);
        }

        /// <summary>
        /// 更新实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        public virtual DataResult Update(TModel model)
        {
            if (IsDulicate(model))
                return DataAction.Duplicate;
            return DataResult.FromResult(Database.Update(model), DataAction.Updated);
        }

        /// <summary>
        /// 更新实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回执行结果。</returns>
        public virtual async Task<DataResult> UpdateAsync(TModel model)
        {
            if (await IsDulicateAsync(model))
                return DataAction.Duplicate;
            return DataResult.FromResult(await Database.UpdateAsync(model), DataAction.Updated);
        }
    }
}