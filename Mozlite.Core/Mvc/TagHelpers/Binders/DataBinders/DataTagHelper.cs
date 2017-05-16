using System;
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

        private void Render(TagHelperOutput output, object model, Element elements)
        {
            foreach (var element in elements)
            {
                SyntaxRender(output, model, element);
            }
        }

        private void SyntaxRender(TagHelperOutput output, object instance, ElementBase element)
        {
            switch (element.Type)
            {
                case ElementType.Text:
                    output.Content.AppendHtml(element.Source);
                    return;
                case ElementType.Block:
                    if (element is CodeBlockElement block && _factory.TryGetSyntax(block.Key, out var syntax))
                    {
                        var syntaxBase = syntax as Syntax;
                        if (syntaxBase != null && syntaxBase.Render == null)
                        {
                            syntaxBase.Render = Render;
                            syntaxBase.GetValue = GetValue;
                        }
                        syntax.Parse(output, block.Condition, instance, block);
                        return;
                    }
                    break;
            }
            string source = null;
            if (element.Source.StartsWith("$") && instance is IEnumerable children)
            {
                switch (element.Source)
                {
                    case "$size":
                        source = children.OfType<object>().Count().ToString();
                        break;
                }
            }
            else
            {
                source = GetValue(instance, element.Source);
            }
            output.Content.AppendHtml(source);
        }
    }
}