namespace Mozlite.Mvc.TagHelpers.Ajax.Syntaxs
{
    /// <summary>
    /// IF条件语句。
    /// </summary>
    public class IfSyntax : ISyntax
    {
        /// <summary>
        /// 关键字。
        /// </summary>
        public string Keyword => "if";

        /// <summary>
        /// 生成语句。
        /// </summary>
        /// <param name="code">条件代码块。</param>
        /// <returns>返回生成的语句。</returns>
        public string Begin(string code)
        {
            return $"if({code}){{";
        }

        /// <summary>
        /// 结束语句。
        /// </summary>
        /// <returns>返回结束语句。</returns>
        public string End()
        {
            return "}";
        }
    }
}