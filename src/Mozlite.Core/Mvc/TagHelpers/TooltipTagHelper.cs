using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// Tooltip标签。
    /// </summary>
    [HtmlTargetElement("i", Attributes = AttributeName)]
    public class TooltipTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        private const string AttributeName = "x-tip";

        /// <summary>
        /// 提示位置。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public Placement Placement { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.MergeClassNames(context, "fa", "fa-question-circle");
            var builder = new TagBuilder("i");
            builder.MergeAttribute("data-toggle", "tooltip");
            builder.MergeAttribute("data-placement", Placement.ToString().ToLower());
            output.MergeAttributes(builder);
        }
    }
}