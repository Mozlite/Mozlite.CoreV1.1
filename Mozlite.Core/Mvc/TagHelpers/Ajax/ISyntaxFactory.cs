using Mozlite.Core;

namespace Mozlite.Mvc.TagHelpers.Ajax
{
    /// <summary>
    /// 语法工厂接口。
    /// </summary>
    public interface ISyntaxFactory : ISingletonService
    {
        /// <summary>
        /// 尝试获取语法。
        /// </summary>
        /// <param name="keyword">关键词。</param>
        /// <param name="syntax">语法接口实例。</param>
        /// <returns>返回获取结果。</returns>
        bool TryGetSyntax(string keyword, out ISyntax syntax);
    }
}