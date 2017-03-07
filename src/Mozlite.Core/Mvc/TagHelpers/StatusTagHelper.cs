using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 状态标签。
    /// </summary>
    [HtmlTargetElement("x:status", Attributes = AttributeName)]
    public class StatusTagHelper : TagHelperBase
    {
        private const string TrueAttributeName = "x-true";
        private const string FalseAttributeName = "x-false";
        private const string AttributeName = "x-value";

        /// <summary>
        /// 正确条件图标名称。
        /// </summary>
        [HtmlAttributeName(TrueAttributeName)]
        public string TrueIconName { get; set; } = "check";

        /// <summary>
        /// 错误条件图标名称。
        /// </summary>
        [HtmlAttributeName(FalseAttributeName)]
        public string FalseIconName { get; set; } = "close";

        /// <summary>
        /// 状态。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public bool Status { get; set; }

        [HtmlAttributeName("x-titled")]
        public bool IsTextTitle { get; set; }

        /// <summary>
        /// 正确显示的文字。
        /// </summary>
        [HtmlAttributeName("x-true-text")]
        public string TrueText { get; set; }

        /// <summary>
        /// 错误显示的文字。
        /// </summary>
        [HtmlAttributeName("x-false-text")]
        public string FalseText { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        /// .
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("x-status");
            var builder = new TagBuilder("i");
            builder.AddCssClass("fa");
            if (Status)
            {
                builder.AddCssClass($"fa-{TrueIconName}");
                wrapper.AddCssClass("text-success");
            }
            else
            {
                builder.AddCssClass($"fa-{FalseIconName}");
                wrapper.AddCssClass("text-danger");
            }
            var text = Status ? TrueText : FalseText;
            output.TagName = "span";
            output.MergeAttributes(wrapper);
            if (IsTextTitle)
            {
                if (!string.IsNullOrWhiteSpace(text))
                    builder.MergeAttribute("title", text);
            }
            else if (string.IsNullOrWhiteSpace(text))
                output.Content.AppendHtml(await output.GetChildContentAsync());
            else
                output.Content.AppendHtml(text);
            output.Content.AppendHtml(builder);
        }
    }
}