namespace Mozlite.Mvc.TagHelpers.Binders.JsonBinders.Syntaxs
{
    /// <summary>
    /// case语法。
    /// </summary>
    public class CaseSyntax : ISyntax
    {
        /// <summary>
        /// 关键字。
        /// </summary>
        public string Keyword => "case";

        /// <summary>
        /// 生成语句。
        /// </summary>
        /// <param name="code">条件代码块。</param>
        /// <returns>返回生成的语句。</returns>
        public string Begin(string code)
        {
            return $"case {code}:{{";
        }

        /// <summary>
        /// 结束语句。
        /// </summary>
        /// <returns>返回结束语句。</returns>
        public string End()
        {
            return "}break;";
        }
    }
}