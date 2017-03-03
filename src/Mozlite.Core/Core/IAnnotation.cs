namespace Mozlite.Core
{
    /// <summary>
    /// 扩展实例接口。
    /// </summary>
    public interface IAnnotation
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 值。
        /// </summary>
        object Value { get; }
    }
}