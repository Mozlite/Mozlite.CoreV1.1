using System;
using System.Collections.Generic;
using System.Linq;

namespace Mozlite.Mvc.TagHelpers.Binders.JsonBinders
{
    /// <summary>
    /// 语法工厂实现类。
    /// </summary>
    public class SyntaxFactory : ISyntaxFactory
    {
        private readonly Dictionary<string, ISyntax> _syntaxs;
        /// <summary>
        /// 初始化类<see cref="SyntaxFactory"/>。
        /// </summary>
        /// <param name="syntaxs">语法列表。</param>
        public SyntaxFactory(IEnumerable<ISyntax> syntaxs)
        {
            _syntaxs = syntaxs.ToDictionary(x => x.Keyword, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 尝试获取语法。
        /// </summary>
        /// <param name="keyword">关键词。</param>
        /// <param name="syntax">语法接口实例。</param>
        /// <returns>返回获取结果。</returns>
        public bool TryGetSyntax(string keyword, out ISyntax syntax)
        {
            return _syntaxs.TryGetValue(keyword, out syntax);
        }
    }
}