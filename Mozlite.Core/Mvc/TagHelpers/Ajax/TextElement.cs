namespace Mozlite.Mvc.TagHelpers.Ajax
{
    /// <summary>
    /// 字符串节点。
    /// </summary>
    public class TextElement : ElementBase
    {
        /// <summary>
        /// 初始化类<see cref="TextElement"/>。
        /// </summary>
        /// <param name="source">源代码。</param>
        public TextElement(string source) : base(source, ElementType.Text)
        {
        }
    }
}