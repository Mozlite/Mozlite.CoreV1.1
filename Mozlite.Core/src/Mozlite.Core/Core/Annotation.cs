
namespace Mozlite.Core
{
    /// <summary>
    /// 扩展实例类。
    /// </summary>
    public class Annotation : IAnnotation
    {
        /// <summary>
        /// 初始化类<see cref="Annotation"/>。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <param name="value">值对象实例。</param>
        public Annotation([NotNull] string name, object value)
        {
            Check.NotEmpty(name, nameof(name));

            Name = name;
            Value = value;
        }

        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// 值。
        /// </summary>
        public virtual object Value { get; }
    }
}