using System.Reflection;
using Mozlite.Data.Metadata.Internal;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 属性扩展类。
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// 获取属性大小。
        /// </summary>
        /// <param name="property">属性实例对象。</param>
        /// <returns>返回当前属性大小。</returns>
        public static int? GetSize(this IProperty property)
        {
            var instance = property as Property;
            var annotation = instance?.GetOrAddAnnotation("MaxLength", () => instance.PropertyInfo.GetCustomAttribute<SizeAttribute>()?.Size);
            return annotation?.Value as int?;
        }

        /// <summary>
        /// 获取行版本配置。
        /// </summary>
        /// <param name="property">属性实例对象。</param>
        /// <returns>返回行版本配置。</returns>
        public static bool? IsRowVersion(this IProperty property)
        {
            var instance = property as Property;
            var annotation = instance?.GetOrAddAnnotation("RowVersion", () => instance.PropertyInfo.IsDefined(typeof(RowVersionAttribute)));
            return annotation?.Value as bool?;
        }
    }
}