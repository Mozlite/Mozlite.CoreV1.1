using System.Collections.Generic;
using Mozlite.Data.Metadata;
using Newtonsoft.Json;

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

        object IParentable.Parent => Parent;
        IEnumerable<object> IParentable.Children => Children;

        /// <summary>
        /// 父级分类。
        /// </summary>
        [Ignore(Ignore.All)]
        [JsonIgnore]
        public TCategory Parent { get; private set; }

        /// <summary>
        /// 获取子项。
        /// </summary>
        public IEnumerable<TCategory> Children => _children;

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

        object IParentable.this[int index] => this[index];

        /// <summary>
        /// 索引获取当前模型实例对象。
        /// </summary>
        /// <param name="index">索引值。</param>
        /// <returns>返回当前模型实例。</returns>
        [JsonIgnore]
        public TCategory this[int index] => _children[index];
    }
}