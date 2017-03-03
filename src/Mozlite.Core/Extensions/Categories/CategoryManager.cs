using System;
using System.Linq;
using Mozlite.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace Mozlite.Extensions.Categories
{
    /// <summary>
    /// 分类管理基类。
    /// </summary>
    /// <typeparam name="TCategory">分类类型。</typeparam>
    public abstract class CategoryManager<TCategory> : ICategoryManager<TCategory> where TCategory : CategoryBase, new()
    {
        /// <summary>
        /// 数据库操作实例对象。
        /// </summary>
        protected readonly IRepository<TCategory> Database;
        private readonly IMemoryCache _cache;
        private readonly Type _cacheKey;

        /// <summary>
        /// 初始化类<see cref="CategoryManager{TCategory}"/>。
        /// </summary>
        /// <param name="repository">类型数据库操作接口。</param>
        /// <param name="cache">换成接口。</param>
        protected CategoryManager(IRepository<TCategory> repository, IMemoryCache cache)
        {
            Database = repository;
            _cache = cache;
            _cacheKey = typeof(TCategory);
        }

        /// <summary>
        /// 从缓存中获取分类列表。
        /// </summary>
        /// <param name="func">从数据库中获取分类列表。</param>
        /// <returns>返回分类列表。</returns>
        protected virtual IEnumerable<TCategory> GetOrAddCategories(Func<IEnumerable<TCategory>> func)
        {
            return _cache.GetOrCreate(_cacheKey, ctx =>
            {
                ctx.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return func();
            });
        }

        /// <summary>
        /// 根据执行结果移除缓存操作后返回操作结果。
        /// </summary>
        /// <param name="result">执行结果。</param>
        /// <param name="succeed">成功时候返回的结果。</param>
        /// <param name="failured">失败时候返回的结果。</param>
        /// <returns>数据库操作结果。</returns>
        protected virtual DataAction Execute(bool result, DataAction succeed, DataAction failured)
        {
            if (result)
            {
                _cache.Remove(_cacheKey);
                return succeed;
            }
            return failured;
        }

        /// <summary>
        /// 保存分类实例对象。
        /// </summary>
        /// <param name="category">分类实例对象。</param>
        /// <returns>返回保存结果。</returns>
        public virtual DataResult Save(TCategory category)
        {
            if (IsDuplicate(category))
                return DataAction.Duplicate;
            if (category.Id > 0)
                return Execute(Database.Update(category), DataAction.Updated, DataAction.UpdatedFailured);
            return Execute(Database.Create(category), DataAction.Created, DataAction.CreatedFailured);
        }

        /// <summary>
        /// 判断分类是否重复。
        /// </summary>
        /// <param name="category">当前分类实例对象。</param>
        /// <returns>返回判断结果。</returns>
        protected virtual bool IsDuplicate(TCategory category)
        {
            return Database.Any(c => c.Name == category.Name && c.Id != category.Id);
        }

        /// <summary>
        /// 通过Id获取分类实例。
        /// </summary>
        /// <param name="id">分类Id。</param>
        /// <returns>返回分类实例对象。</returns>
        public virtual TCategory Get(int id)
        {
            return Database.Find(c => c.Id == id);
        }

        /// <summary>
        /// 通过名称获取分类实例。
        /// </summary>
        /// <param name="name">分类名称。</param>
        /// <returns>返回分类实例对象。</returns>
        public virtual TCategory Get(string name)
        {
            return Database.Find(c => c.Name == name);
        }

        /// <summary>
        /// 通过Id删除分类。
        /// </summary>
        /// <param name="id">分类Id。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(int id)
        {
            return Execute(Database.Delete(c => c.Id == id), DataAction.Deleted, DataAction.DeletedFailured);
        }

        /// <summary>
        /// 通过Id删除分类。
        /// </summary>
        /// <param name="ids">分类Id集合。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(int[] ids)
        {
            return Execute(Database.Delete(c => c.Id.Included(ids)), DataAction.Deleted, DataAction.DeletedFailured);
        }

        /// <summary>
        /// 通过Id删除分类。
        /// </summary>
        /// <param name="ids">分类Id集合。</param>
        /// <returns>返回删除结果。</returns>
        public DataResult Delete(string ids)
        {
            return Delete(ids.SplitToInt32());
        }

        /// <summary>
        /// 加载所有分类。
        /// </summary>
        /// <returns>分类列表。</returns>
        public virtual IEnumerable<TCategory> Load()
        {
            return Database.Load();
        }

        /// <summary>
        /// 从缓存中获取分类实例对象。
        /// </summary>
        /// <param name="id">分类Id。</param>
        /// <returns>返回分类实例对象。</returns>
        public virtual TCategory GetCache(int id)
        {
            return LoadCaches().SingleOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// 从缓存中通过名称获取分类实例。
        /// </summary>
        /// <param name="name">分类名称。</param>
        /// <returns>返回分类实例对象。</returns>
        public virtual TCategory GetCache(string name)
        {
            return LoadCaches().SingleOrDefault(c => c.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        /// <summary>
        /// 加载缓存中的所有分类。
        /// </summary>
        /// <returns>分类列表。</returns>
        public virtual IEnumerable<TCategory> LoadCaches()
        {
            return GetOrAddCategories(Load);
        }

        /// <summary>
        /// 从缓存中通过名称获取分类Id。
        /// </summary>
        /// <param name="name">分类名称。</param>
        /// <returns>返回分类Id。</returns>
        public virtual int GetId(string name)
        {
            return GetCache(name)?.Id ?? 0;
        }

        /// <summary>
        /// 从缓存中通过Id获取分类名称。
        /// </summary>
        /// <param name="id">分类Id。</param>
        /// <returns>返回分类名称。</returns>
        public virtual string GetName(int id)
        {
            return GetCache(id)?.Name;
        }
    }
}