using System;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mozlite.Mvc.TagHelpers.Binders.DataBinders
{
    /// <summary>
    /// 递归呈现。
    /// </summary>
    public class ChildrenSyntax : Syntax
    {
        /// <summary>
        /// 关键字。
        /// </summary>
        public override string Keyword => "children";

        /// <summary>
        /// 生成语句。
        /// </summary>
        /// <param name="output">输出标签实例。</param>
        /// <param name="code">条件代码块。</param>
        /// <param name="instance">当前实例对象。</param>
        /// <param name="element">当前节点实例。</param>
        /// <returns>返回生成的语句。</returns>
        public override void Parse(TagHelperOutput output, string code, object instance, CodeBlockElement element)
        {
            if (instance is IEnumerable children)
            {
                var current = element.Any() ? element as Element : element.Doc;
                foreach (var child in children)
                {
                    Render(output, child, current);
                }
            }
        }
    }
}