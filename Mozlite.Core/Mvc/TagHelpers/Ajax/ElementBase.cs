namespace Mozlite.Mvc.TagHelpers.Ajax
{
    /// <summary>
    /// 节点基类。
    /// </summary>
    public abstract class ElementBase
    {
        /// <summary>
        /// 父级节点。
        /// </summary>
        public Element Parent { get; set; }

        /// <summary>
        /// 原字符串。
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// 节点类型。
        /// </summary>
        public ElementType Type { get; }

        /// <summary>
        /// 初始化类<see cref="Element"/>。
        /// </summary>
        /// <param name="source">源代码。</param>
        /// <param name="type">类型。</param>
        protected ElementBase(string source, ElementType type)
        {
            Source = source;
            Type = type;
        }

        /// <summary>返回表示当前对象的字符串。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString() => Source;
    }
}