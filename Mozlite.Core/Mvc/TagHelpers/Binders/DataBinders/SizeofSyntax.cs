using System;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mozlite.Mvc.TagHelpers.Binders.DataBinders
{
    /// <summary>
    /// 判断当前记录数大小，参数：
    /// +1：表示>1；
    /// -1：表示&lt;1；
    /// 1：表示==1。
    /// </summary>
    public class SizeofSyntax : Syntax
    {
        /// <summary>
        /// 关键字。
        /// </summary>
        public override string Keyword => "sizeof";

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
                var size = children.Cast<object>().Count();
                var current = 0;
                switch (code[0])
                {
                    case '-':
                        if (!int.TryParse(code.Substring(1), out current) || size >= current)
                            return;
                        break;
                    case '+':
                        if (!int.TryParse(code.Substring(1), out current) || size <= current)
                            return;
                        break;
                    default:
                        if (size.ToString() != code)
                            return;
                        break;
                }
                Render(output, instance, element);
            }
        }
    }
}