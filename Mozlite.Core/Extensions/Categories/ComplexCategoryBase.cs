using System.Collections;
using System.Collections.Generic;
using Mozlite.Data.Metadata;

namespace Mozlite.Extensions.Categories
{
    /// <summary>
    /// 多级分类基类。
    /// </summary>
    /// <typeparam name="TCategory">分类实例类型。</typeparam>
    public abstract class ComplexCategoryBase<TCategory> : CategoryBase, IParentable<TCategory>
        where TCategory : ComplexCategoryBase<TCategory>, new()
    {
        private readonly IList<TCategory> _children = new List<TCategory>();

        /// <summary>
        /// 父级类型ID。
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 父级分类。
        /// </summary>
        [Ignore(Ignore.All)]
        public TCategory Parent { get; private set; }

        /// <summary>返回一个循环访问集合的枚举器。</summary>
        /// <returns>用于循环访问集合的枚举数。</returns>
        public IEnumerator<TCategory> GetEnumerator()
        {
            return _children.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 添加分类。
        /// </summary>
        /// <param name="category">分类实例对象。</param>
        public void Add(TCategory category)
        {
            category.ParentId = Id;
            category.Parent = (TCategory)this;
            _children.Add(category);
        }

        /// <summary>
        /// 子级数量。
        /// </summary>
        public int Count => _children.Count;
    }
}