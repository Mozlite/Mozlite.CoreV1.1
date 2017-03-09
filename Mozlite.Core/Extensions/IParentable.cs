using System.Collections.Generic;

namespace Mozlite.Extensions
{
    /// <summary>
    /// 递归接口。
    /// </summary>
    public interface IParentable<TModel> : IEnumerable<TModel>
        where TModel : IParentable<TModel>
    {
        /// <summary>
        /// 唯一Id。
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 父级Id。
        /// </summary>
        int ParentId { get; }

        /// <summary>
        /// 父级实例。
        /// </summary>
        TModel Parent { get; }

        /// <summary>
        /// 添加子集实例。
        /// </summary>
        /// <param name="model">子集实例。</param>
        void Add(TModel model);
    }
}