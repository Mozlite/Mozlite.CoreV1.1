using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Mozlite.Extensions.Categories;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 多级分类下拉列表。
    /// </summary>
    public abstract class ComplexCategorySelectListTagHelper<TCategory> : SelectableTagHelperBase
        where TCategory : ComplexCategoryBase<TCategory>, new()
    {
        /// <summary>
        /// 初始化类<see cref="ComplexCategorySelectListTagHelper{TCategory}"/>。
        /// </summary>
        /// <param name="generator">Html辅助接口。</param>
        protected ComplexCategorySelectListTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        /// <summary>
        /// 分类试图数据名称。
        /// </summary>
        [HtmlAttributeName("x-name")]
        public string ViewBagName { get; set; } = "Categories";

        /// <summary>
        /// 分类列表。
        /// </summary>
        [HtmlAttributeName("x-items")]
        public IEnumerable<TCategory> Items { get; set; }

        /// <summary>
        /// 不包含的Id，如移除自身以及子集的所有分类。
        /// </summary>
        [HtmlAttributeName("x-exclude")]
        public int? Exclude { get; set; }

        /// <summary>
        /// 添加下拉列表框选项。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="items">下拉列表框列表。</param>
        protected override void Init(List<SelectListItem> items, TagHelperContext context)
        {
            var categories = (Items ?? ViewContext.ViewData[ViewBagName] as IEnumerable<TCategory>)?.ToList();
            if (categories != null)
            {
                var isFirst = string.IsNullOrWhiteSpace(Text);
                var id = context.AllAttributes["value"]?.Value.ToString().AsInt32();
                for (int i = 0; i < categories.Count; i++)
                {
                    AppendCategory(items, categories[i], id, i + 1 == categories.Count, isFirst: isFirst);
                }
            }
        }

        /// <summary>
        /// 获取显示的字符串。
        /// </summary>
        /// <param name="category">分类实例。</param>
        /// <returns>返回显示的字符串。</returns>
        protected virtual string SelectText(TCategory category)
        {
            return category.Name;
        }

        private string SelectText(TCategory category, bool isEnd, string header, bool isFirst)
        {
            if (isFirst)
                return SelectText(category);
            var sb = new StringBuilder();
            sb.Append(header);
            if (isEnd)
                sb.Append("└─ ");
            else
                sb.Append("├─ ");
            sb.Append(SelectText(category));
            return sb.ToString();
        }

        private void AppendCategory(List<SelectListItem> items, TCategory category, int? value, bool isEnd, string header = null, bool isFirst = false)
        {
            if (Exclude != null && category.Id == Exclude.Value)
            {
                if (isEnd && items.Count > 1)
                {
                    var text = items[items.Count - 1].Text;
                    var charAt = text.LastIndexOf('├');
                    if (charAt != -1)
                        text = text.Remove(charAt, 1).Insert(charAt, "└");
                    items[items.Count - 1].Text = text;
                }
                return;
            }
            items.Add(new SelectListItem { Text = SelectText(category, isEnd, header, isFirst), Value = category.Id.ToString(), Selected = value == category.Id });
            var children = category.Children.ToList();
            if (!isEnd)
                header += "│ ";
            header += "\u3000";
            for (int i = 0; i < children.Count; i++)
            {
                AppendCategory(items, children[i], value, i + 1 == children.Count, header);
            }
        }
    }
}