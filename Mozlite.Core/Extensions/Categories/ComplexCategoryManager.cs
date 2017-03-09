using System.Linq;
using Mozlite.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace Mozlite.Extensions.Categories
{
    /// <summary>
    /// 多级分类管理基类。
    /// </summary>
    /// <typeparam name="TCategory">分类类型。</typeparam>
    public abstract class ComplexCategoryManager<TCategory> : CategoryManager<TCategory>, IComplexCategoryManager<TCategory> where TCategory : ComplexCategoryBase<TCategory>, new()
    {
        /// <summary>
        /// 初始化类<see cref="ComplexCategoryManager{TCategory}"/>。
        /// </summary>
        /// <param name="repository">数据库操作接口。</param>
        /// <param name="cache">缓存接口。</param>
        protected ComplexCategoryManager(IRepository<TCategory> repository, IMemoryCache cache)
            : base(repository, cache)
        {
        }

        /// <summary>
        /// 加载当前分类子列表。
        /// </summary>
        /// <param name="id">当前分类Id。</param>
        /// <returns>返回分类包含列表。</returns>
        public TCategory GetChildren(int id)
        {
            var categories = Database.AsQueryable().LoadChildren(c => c.Id == id);
            return GetCategory(categories, id);
        }

        private TCategory GetCategory(IEnumerable<TCategory> categories, int id)
        {
            var dic = categories.ToDictionary(c => c.Id);
            dic[0] = new TCategory();
            foreach (var category in categories)
            {
                TCategory temp;
                if (dic.TryGetValue(category.ParentId, out temp))
                    temp.Add(category);
            }
            return dic[id];
        }

        /// <summary>
        /// 加载当前分类的父级分类。
        /// </summary>
        /// <param name="id">当前分类Id。</param>
        /// <returns>返回分类包含列表。</returns>
        public TCategory GetParent(int id)
        {
            var categories = Database.AsQueryable().LoadParents(c => c.Id == id);
            return GetCategory(categories, 0);
        }

        /// <summary>
        /// 加载缓存中的所有分类。
        /// </summary>
        /// <returns>分类列表。</returns>
        public override IEnumerable<TCategory> LoadCaches()
        {
            return GetOrAddCategories(() =>
            {
                var categories = Load();
                var dic = categories.ToDictionary(c => c.Id);
                dic[0] = new TCategory();
                foreach (var category in categories)
                {
                    TCategory temp;
                    if (dic.TryGetValue(category.ParentId, out temp))
                        temp.Add(category);
                }
                return dic.Values;
            });
        }

        /// <summary>
        /// 通过Id删除分类。
        /// </summary>
        /// <param name="id">分类Id。</param>
        /// <returns>返回删除结果。</returns>
        public override DataResult Delete(int id)
        {
            var current = GetCache(id);
            if (current.Any())
                return DataAction.DeletedFailuredItemsNotEmpty;
            return base.Delete(id);
        }

        /// <summary>
        /// 通过Id删除分类。
        /// </summary>
        /// <param name="ids">分类Id集合。</param>
        /// <returns>返回删除结果。</returns>
        public override DataResult Delete(int[] ids)
        {
            foreach (var id in ids)
            {
                var current = GetCache(id);
                if (current.Any())
                    return DataAction.DeletedFailuredItemsNotEmpty;
            }
            return base.Delete(ids);
        }
    }
}