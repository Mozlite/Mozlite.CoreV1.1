namespace Mozlite.Extensions.Categories
{
    /// <summary>
    /// 多级分类管理接口。
    /// </summary>
    /// <typeparam name="TCategory">分类类型。</typeparam>
    public interface IComplexCategoryManager<TCategory> : ICategoryManager<TCategory> where TCategory : ComplexCategoryBase<TCategory>, new()
    {

    }
}