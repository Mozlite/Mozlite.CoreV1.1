using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 所有标签。
    /// </summary>
    [HtmlTargetElement("*", Attributes = NullAttributeName)]
    [HtmlTargetElement("*", Attributes = AttributeName)]
    public class HiddenTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        private const string AttributeName = "x-hidden";
        private const string NullAttributeName = "x-null-hidden";
        /// <summary>
        /// 是否隐藏。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public bool? IsHidden { get; set; }

        /// <summary>
        /// 判断对象。
        /// </summary>
        [HtmlAttributeName(NullAttributeName)]
        public object NullObject { get; set; }
        
        /// <inheritdoc />
        public override int Order => int.MaxValue;

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var hidden = IsHidden ?? (NullObject == null || string.IsNullOrWhiteSpace(NullObject.ToString()));
            if (hidden)
            {
                output.SuppressOutput();
            }
        }
    }
}