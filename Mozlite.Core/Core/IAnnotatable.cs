using System.Collections.Generic;

namespace Mozlite.Core
{
    /// <summary>
    /// 可存储扩展属性的对象接口。
    /// </summary>
    public interface IAnnotatable
    {
        /// <summary>
        /// 获取当前名称<paramref name="name"/>的存储对象实例，如果没找到返回null。
        /// </summary>
        /// <param name="name">存储名称。</param>
        /// <returns>存储对象实例，如果没找到返回null。</returns>
        object this[[NotNull] string name] { get; }

        /// <summary>
        /// 获取当前名称<paramref name="name"/>的存储对象实例，如果没找到返回null。
        /// </summary>
        /// <param name="name">存储名称。</param>
        /// <returns>存储对象实例，如果没找到返回null。</returns>
        IAnnotation FindAnnotation([NotNull] string name);

        /// <summary>
        /// 获取当前对象的所有扩展存储实例列表。
        /// </summary>
        /// <returns>所有扩展存储实例列表。</returns>
        IEnumerable<IAnnotation> GetAnnotations();

        /// <summary>
        /// 获取扩展对象实例值。
        /// </summary>
        /// <typeparam name="TValue">值类型。</typeparam>
        /// <param name="name">扩展名称。</param>
        /// <returns>返回值实例对象。</returns>
        TValue GetAnnotation<TValue>([NotNull] string name);
    } 
}