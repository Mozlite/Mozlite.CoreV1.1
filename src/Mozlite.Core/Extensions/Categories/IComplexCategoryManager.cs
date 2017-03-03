namespace Mozlite.Extensions.Categories
{
    /// <summary>
    /// 多级分类管理接口。
    /// </summary>
    /// <typeparam name="TCategory">分类类型。</typeparam>
    public interface IComplexCategoryManager<TCategory> : ICategoryManager<TCategory> where TCategory : ComplexCategoryBase<TCategory>, new()
    {
        /// <summary>
        /// 加载当前分类子列表。
        /// </summary>
        /// <param name="id">当前分类Id。</param>
        /// <returns>返回分类包含列表。</returns>
        TCategory GetChildren(int id);

        /// <summary>
        /// 加载当前分类的父级分类。
        /// </summary>
        /// <param name="id">当前分类Id。</param>
        /// <returns>返回分类包含列表。</returns>
        TCategory GetParent(int id);
    }
}