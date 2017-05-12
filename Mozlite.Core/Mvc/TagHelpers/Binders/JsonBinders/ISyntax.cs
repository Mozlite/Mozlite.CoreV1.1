using Mozlite.Core;

namespace Mozlite.Mvc.TagHelpers.Binders.JsonBinders
{
    /// <summary>
    /// 语法接口。
    /// </summary>
    public interface ISyntax : IServices
    {
        /// <summary>
        /// 关键字。
        /// </summary>
        string Keyword { get; }

        /// <summary>
        /// 生成语句。
        /// </summary>
        /// <param name="code">条件代码块。</param>
        /// <returns>返回生成的语句。</returns>
        string Begin(string code);

        /// <summary>
        /// 结束语句。
        /// </summary>
        /// <returns>返回结束语句。</returns>
        string End();
    }
}