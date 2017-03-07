using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 当前激活标签。
    /// </summary>
    [HtmlTargetElement("*", Attributes = AttributeName + "," + ValueAttributeName)]
    public class ActiveTagHelper : ViewContextableTagHelperBase
    {
        private const string AttributeName = "x-current";
        private const string ValueAttributeName = "x-value";
        private const string ClassAttributeName = "x-class";
        /// <summary>
        /// 当前键值。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string Current { get; set; }

        /// <summary>
        /// 当前值。
        /// </summary>
        [HtmlAttributeName(ValueAttributeName)]
        public string Value { get; set; }

        /// <summary>
        /// 样式名称。
        /// </summary>
        [HtmlAttributeName(ClassAttributeName)]
        public string ClassName { get; set; } = "active";

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            if (Current == null)
                Current = ViewContext.ViewBag.Current;
            if (string.Compare(Current, Value, StringComparison.OrdinalIgnoreCase) == 0)
                output.MergeClassNames(context, ClassName);
        }
    }
}