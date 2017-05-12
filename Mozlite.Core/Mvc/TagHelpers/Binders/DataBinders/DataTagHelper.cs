using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Mozlite.Data.Metadata;

namespace Mozlite.Mvc.TagHelpers.Binders.DataBinders
{
    /// <summary>
    /// 模板类型标签。
    /// </summary>
    [HtmlTargetElement("*", Attributes = "[jmoz-binder=data]")]
    public class DataTagHelper : TagHelperBase
    {
        private readonly ISyntaxFactory _factory;
        private readonly IModel _model;
        private IEntityType _entityType;

        /// <summary>
        /// 获取当前对象中<paramref name="propertyName"/>的值。
        /// </summary>
        /// <param name="model">模型实例。</param>
        /// <param name="propertyName">属性名称。</param>
        /// <returns>返回当前属性的值。</returns>
        protected string GetValue(object model, string propertyName)
        {
            _entityType = _entityType ?? _model.GetEntity(model.GetType());
            return _entityType?.FindProperty(propertyName)?.Get(model)?.ToString();
        }

        /// <summary>
        /// 初始化类<see cref="DataTagHelper"/>。
        /// </summary>
        /// <param name="factory">语法工厂接口。</param>
        /// <param name="model">模型缓存实例。</param>
        public DataTagHelper(ISyntaxFactory factory, IModel model)
        {
            _factory = factory;
            _model = model;
        }

        /// <summary>
        /// 当前模型实例。
        /// </summary>
        [HtmlAttributeName("jmoz-binder")]
        public string Binder { get; set; }

        /// <summary>
        /// 当前模型实例。
        /// </summary>
        [HtmlAttributeName("jmoz-models")]
        public IEnumerable Models { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        /// .
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var template = await output.GetChildContentAsync();
            if (template.IsEmptyOrWhiteSpace || Models == null)
            {
                output.SuppressOutput();
                return;
            }

            var source = template.GetContent();
            var document = new DocumentElement(source);
            foreach (var model in Models)
            {
                Render(output, model, document);
            }
        }

        private void Render(TagHelperOutput output, object model, DocumentElement document)
        {
            foreach (var element in document)
            {
                SyntaxRender(output, model, element, document);
            }
        }

        private void SyntaxRender(TagHelperOutput output, object instance, ElementBase element, DocumentElement document)
        {
            switch (element.Type)
            {
                case ElementType.Text:
                    output.Content.AppendHtml(element.Source);
                    return;
                case ElementType.Block:
                    if (element is CodeBlockElement block && block.Key == "children" && instance is IEnumerable items)
                    {
                        foreach (var item in items)
                        {
                            Render(output, item, document);
                        }
                        return;
                    }
                    break;
            }
            output.Content.AppendHtml(GetValue(instance, element.Source));
        }

        //private readonly Regex _regex = new Regex("{{\\s*([a-z_0-9]+)?(\\s*:\\s*'(.*)')?\\s*}}", RegexOptions.IgnoreCase);

        ///// <summary>
        ///// 尝试匹配。
        ///// </summary>
        ///// <param name="template">模板。</param>
        ///// <param name="model">模型实例。</param>
        ///// <param name="html">替换出来的HTML代码。</param>
        ///// <returns>返回匹配结果。</returns>
        //protected virtual bool TryParse(string template, object model, out string html)
        //{
        //    html = _regex.Replace(template, match =>
        //    {
        //        var key = match.Groups[1].Value.Trim();
        //        var value = GetValue(model, key);
        //        if (string.IsNullOrWhiteSpace(value))
        //            value = match.Groups[3].Value.Trim();
        //        return value;
        //    });
        //    return true;
        //}
    }
}