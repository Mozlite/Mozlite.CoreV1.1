using System.Collections.Generic;

namespace Mozlite.Core
{
    /// <summary>
    /// 扩展接口，可以修改扩展实例对象。
    /// </summary>
    public interface IMutableAnnotatable : IAnnotatable
    {
        /// <summary>
        /// 获取或设置扩展实例。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <returns>返回扩展实例值。</returns>
        new object this[[NotNull] string name] { get; [param: CanBeNull] set; }

        /// <summary>
        /// 获取所有扩展实例列表。
        /// </summary>
        /// <returns>所有扩展实例列表。</returns>
        new IEnumerable<Annotation> GetAnnotations();

        /// <summary>
        /// 添加一个扩展到对象中，如果已经存在扩展名称将抛出错误。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <param name="value">值。</param>
        /// <returns>返回当前添加的扩展实例。</returns>
        Annotation AddAnnotation([NotNull] string name, [NotNull] object value);
        
        /// <summary>
        /// 从对象中查找一个名称为<paramref name="name"/>的扩展实例。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <returns>返回得到的扩展实例，如果不存在返回null。</returns>
        new Annotation FindAnnotation([NotNull] string name);

        /// <summary>
        /// 删除对象中的扩展实例。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <returns>返回被删除的扩展实例。</returns>
        Annotation RemoveAnnotation([NotNull] string name);
    }
}