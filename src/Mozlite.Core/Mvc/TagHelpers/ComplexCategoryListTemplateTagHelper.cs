using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Mozlite.Extensions.Categories;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 多级分类下拉列表。
    /// </summary>
    public abstract class ComplexCategoryListTemplateTagHelper<TCategory> : ViewContextableTagHelper
        where TCategory : ComplexCategoryBase<TCategory>, new()
    {
        private class Alt
        {
            private readonly IDictionary<string, string> _items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            public Alt Set(string name, string text)
            {
                _items[name] = text;
                return this;
            }

            public Alt Set(string name, object text)
            {
                _items[name] = text.ToString();
                return this;
            }

            public Alt Set(TCategory category)
            {
                Set("name", category.Name);
                Set("id", category.Id);
                Set("pid", category.ParentId);
                return Set("desc", category.Description);
            }

            public string Get(string name)
            {
                string value;
                _items.TryGetValue(name, out value);
                return value;
            }

            public void WriteTo(TagHelperOutput output, string html)
            {
                foreach (var item in _items)
                {
                    html = html.Replace($"{{:{item.Key}:}}", item.Value);
                }
                output.Content.AppendHtml(html);
            }
        }

        /// <summary>
        /// 分类列表。
        /// </summary>
        [HtmlAttributeName("x-parent")]
        public TCategory Category { get; set; }

        /// <summary>
        /// 不包含的Id，如移除自身以及子集的所有分类。
        /// </summary>
        [HtmlAttributeName("x-exclude")]
        public int? Exclude { get; set; }

        /// <summary>
        /// 标签。
        /// </summary>
        [HtmlAttributeName("x-tag")]
        public string TagName { get; set; } = "div";

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        /// .
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Category = Category ?? ViewContext.ViewData.Model as TCategory;
            if (Category == null)
            {
                output.SuppressOutput();
                return;
            }
            var current = Category.ToList();
            var categories = new List<Alt>();
            for (int i = 0; i < current.Count; i++)
            {
                AppendCategory(categories, current[i], i + 1 == current.Count, isFirst: true);
            }
            var innerHtml = await output.GetChildContentAsync();
            if (innerHtml.IsEmptyOrWhiteSpace)
            {
                output.SuppressOutput();
                return;
            }
            output.TagName = TagName ?? "div";
            output.Content.Clear();
            var htmlTemplete = innerHtml.GetContent();
            foreach (var category in categories)
            {
                category.WriteTo(output, htmlTemplete);
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

        private void AppendCategory(List<Alt> items, TCategory category, bool isEnd, string header = null, bool isFirst = false)
        {
            if (Exclude != null && category.Id == Exclude.Value)
            {
                if (isEnd && items.Count > 1)
                {
                    var text = items[items.Count - 1].Get("text");
                    var charAt = text.LastIndexOf('├');
                    if (charAt != -1)
                        text = text.Remove(charAt, 1).Insert(charAt, "└");
                    items[items.Count - 1].Set("text", text);
                }
                return;
            }
            items.Add(new Alt().Set(category).Set("text", SelectText(category, isEnd, header, isFirst)));
            var children = category.ToList();
            if (!isEnd)
                header += "│ ";
            header += "\u3000";
            for (int i = 0; i < children.Count; i++)
            {
                AppendCategory(items, children[i], i + 1 == children.Count, header);
            }
        }
    }
}