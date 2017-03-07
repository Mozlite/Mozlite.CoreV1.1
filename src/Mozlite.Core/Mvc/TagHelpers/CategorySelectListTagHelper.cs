using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Mozlite.Extensions.Categories;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 分类下拉列表框。
    /// </summary>
    [HtmlTargetElement("x:categoryselect")]
    public class CategorySelectListTagHelper : SelectableTagHelper
    {
        private const string AttributeName = "x-items";

        /// <summary>
        /// 分类试图数据名称。
        /// </summary>
        [HtmlAttributeName("x-name")]
        public string ViewBagName { get; set; } = "Categories";

        /// <summary>
        /// 分类列表。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public IEnumerable<CategoryBase> Items { get; set; }

        /// <summary>
        /// 初始化类<see cref="SelectableTagHelper"/>。
        /// </summary>
        /// <param name="generator">Html辅助接口。</param>
        public CategorySelectListTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        /// <summary>
        /// 添加下拉列表框选项。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="items">下拉列表框列表。</param>
        protected override void Init(List<SelectListItem> items, TagHelperContext context)
        {
            var categories = Items ?? ViewContext.ViewData[ViewBagName] as IEnumerable<CategoryBase>;
            if (categories != null)
            {
                var id = context.AllAttributes["value"]?.Value.ToString().AsInt32();
                foreach (var category in categories)
                {
                    items.Add(new SelectListItem { Text = category.Name, Value = category.Id.ToString(), Selected = id == category.Id });
                }
            }
        }
    }
}