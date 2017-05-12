namespace Mozlite.Mvc.TagHelpers.Binders
{
    /// <summary>
    /// 代码节点。
    /// </summary>
    public class CodeElement : ElementBase
    {
        /// <summary>
        /// 初始化类<see cref="CodeElement"/>。
        /// </summary>
        /// <param name="source">源代码。</param>
        public CodeElement(string source) : base(source, ElementType.Code)
        {
        }

        /// <summary>返回表示当前对象的字符串。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            return "{{" + Source + "}}";
        }
    }
}