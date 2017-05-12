using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 标签扩展类。
    /// </summary>
    public static class TagHelperExtensions
    {
        /// <summary>
        /// 合并并添加样式名称。
        /// </summary>
        /// <param name="output">表情输出实例对象。</param>
        /// <param name="context">当前标签上下文实例对象。</param>
        /// <param name="classNames">需要添加合并的样式名称。</param>
        public static void MergeClassNames(this TagHelperOutput output, TagHelperContext context,
            params string[] classNames)
        {
            if (classNames == null) return;
            var htmlClassNames = context.GetClassNames();
            htmlClassNames.AddRange(classNames);
            output.Attributes.SetAttribute("class", string.Join(" ", htmlClassNames.Distinct()));
        }
        
        private static string GetAttribute(this TagHelperContext context, string attributeName)
        {
            return context.AllAttributes[attributeName]?.Value?.ToString();
        }
        
        private static List<string> GetClassNames(this TagHelperContext context)
        {
            return context.GetAttribute("class")?.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList() ??
                   new List<string>();
        }

        /// <summary>
        /// 获取当前标签上下文中的样式属性，并添加样式。
        /// </summary>
        /// <param name="context">当前标签上下文实例对象。</param>
        /// <param name="style">样式字符串。</param>
        public static string GetAndAppendStyle(this TagHelperContext context, string style)
        {
            var innerStyle = context.AllAttributes["style"]?.Value.ToString().Trim();
            if (innerStyle != null && !innerStyle.EndsWith(";"))
                innerStyle += ";";
            innerStyle += style;
            return innerStyle;
        }
    }
}