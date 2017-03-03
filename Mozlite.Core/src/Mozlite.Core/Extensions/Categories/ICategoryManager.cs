using System.Collections.Generic;

namespace Mozlite.Extensions.Categories
{
    /// <summary>
    /// 分类管理接口。
    /// </summary>
    /// <typeparam name="TCategory">分类类型。</typeparam>
    public interface ICategoryManager<TCategory>
        where TCategory : CategoryBase, new()
    {
        /// <summary>
        /// 保存分类实例对象。
        /// </summary>
        /// <param name="category">分类实例对象。</param>
        /// <returns>返回保存结果。</returns>
        DataResult Save(TCategory category);

        /// <summary>
        /// 通过Id获取分类实例。
        /// </summary>
        /// <param name="id">分类Id。</param>
        /// <returns>返回分类实例对象。</returns>
        TCategory Get(int id);

        /// <summary>
        /// 通过名称获取分类实例。
        /// </summary>
        /// <param name="name">分类名称。</param>
        /// <returns>返回分类实例对象。</returns>
        TCategory Get(string name);

        /// <summary>
        /// 通过Id删除分类。
        /// </summary>
        /// <param name="id">分类Id。</param>
        /// <returns>返回删除结果。</returns>
        DataResult Delete(int id);

        /// <summary>
        /// 通过Id删除分类。
        /// </summary>
        /// <param name="ids">分类Id集合。</param>
        /// <returns>返回删除结果。</returns>
        DataResult Delete(int[] ids);

        /// <summary>
        /// 通过Id删除分类。
        /// </summary>
        /// <param name="ids">分类Id集合。</param>
        /// <returns>返回删除结果。</returns>
        DataResult Delete(string ids);

        /// <summary>
        /// 加载所有分类。
        /// </summary>
        /// <returns>分类列表。</returns>
        IEnumerable<TCategory> Load();

        /// <summary>
        /// 从缓存中获取分类实例对象。
        /// </summary>
        /// <param name="id">分类Id。</param>
        /// <returns>返回分类实例对象。</returns>
        TCategory GetCache(int id);

        /// <summary>
        /// 从缓存中通过名称获取分类实例。
        /// </summary>
        /// <param name="name">分类名称。</param>
        /// <returns>返回分类实例对象。</returns>
        TCategory GetCache(string name);

        /// <summary>
        /// 加载缓存中的所有分类。
        /// </summary>
        /// <returns>分类列表。</returns>
        IEnumerable<TCategory> LoadCaches();

        /// <summary>
        /// 从缓存中通过名称获取分类Id。
        /// </summary>
        /// <param name="name">分类名称。</param>
        /// <returns>返回分类Id。</returns>
        int GetId(string name);

        /// <summary>
        /// 从缓存中通过Id获取分类名称。
        /// </summary>
        /// <param name="id">分类Id。</param>
        /// <returns>返回分类名称。</returns>
        string GetName(int id);
    }
}