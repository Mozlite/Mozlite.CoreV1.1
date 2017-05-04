using System;

namespace Mozlite.Mvc.TagHelpers.Ajax.Syntaxs
{
    /// <summary>
    /// Foreach条件语句。
    /// </summary>
    public class ForeachSyntax : ISyntax
    {
        /// <summary>
        /// 关键字。
        /// </summary>
        public string Keyword => "foreach";

        /// <summary>
        /// 生成语句。
        /// </summary>
        /// <param name="code">条件代码块。</param>
        /// <returns>返回生成的语句。</returns>
        public string Begin(string code)
        {
            var items = code.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length == 3 && items[1] == "in")
                return $"{items[2]}.forEach(function({items[0]},index){{";
            throw new Exception("语法错误，必须为{{foreach xx in xxs}}!");
        }

        /// <summary>
        /// 结束语句。
        /// </summary>
        /// <returns>返回结束语句。</returns>
        public string End()
        {
            return "});";
        }
    }
}