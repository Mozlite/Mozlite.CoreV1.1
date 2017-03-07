using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 下拉列表框基类。
    /// </summary>
    public abstract class SelectableTagHelper : ViewContextableTagHelper
    {
        private const string AttributeName = "x-first-text";
        private const string ValueAttributeName = "x-first-value";

        /// <summary>
        /// “请选择”字符串。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string Text { get; set; }

        /// <summary>
        /// “请选择”对应的值。
        /// </summary>
        [HtmlAttributeName(ValueAttributeName)]
        public string Value { get; set; }
        
        /// <summary>
        /// 模型表达式。
        /// </summary>
        [HtmlAttributeName("x-for")]
        public ModelExpression For { get; set; }
        
        private readonly SelectTagHelper _selectTagHelper;
        /// <summary>
        /// 初始化类<see cref="SelectableTagHelper"/>。
        /// </summary>
        /// <param name="generator">Html辅助接口。</param>
        protected SelectableTagHelper(IHtmlGenerator generator)
        {
            _selectTagHelper = new SelectTagHelper(generator);
        }

        /// <summary>
        /// 添加下拉列表框选项。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="items">下拉列表框列表。</param>
        protected abstract void Init(List<SelectListItem> items, TagHelperContext context);
        
        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            var items = new List<SelectListItem>();
            if (Text != null)
                items.Add(new SelectListItem { Text = Text, Value = Value });
            Init(items, context);
            _selectTagHelper.ViewContext = ViewContext;
            _selectTagHelper.Items = items;
            _selectTagHelper.For = For;
            _selectTagHelper.Init(context);
        }
        
        /// <summary>
        /// 呈现输出。
        /// </summary>
        /// <param name="context">标签上下文。</param>
        /// <param name="output">输出上下文。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            _selectTagHelper.Process(context, output);
        }
    }
}