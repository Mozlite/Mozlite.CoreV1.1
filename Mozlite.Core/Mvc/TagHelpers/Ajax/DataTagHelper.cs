using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using Mozlite.Core.Caching;
using Newtonsoft.Json;

namespace Mozlite.Mvc.TagHelpers.Ajax
{
    /// <summary>
    /// 数据标签。
    /// </summary>
    [HtmlTargetElement("*", Attributes = AttributeName)]
    public class DataTagHelper : ViewContextableTagHelperBase
    {
        private readonly ISyntaxFactory _factory;
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// 初始化类<see cref="DataTagHelper"/>。
        /// </summary>
        /// <param name="factory">语法工厂接口。</param>
        /// <param name="cacheManager">缓存接口。</param>
        public DataTagHelper(ISyntaxFactory factory, ICacheManager cacheManager)
        {
            _factory = factory;
            _cacheManager = cacheManager;
        }

        private const string AttributeName = "jmoz-ajax";
        /// <summary>
        /// 当前数据路径。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string Url { get; set; }

        /// <summary>
        /// 下一次获取数据的时间间隔（秒）。
        /// </summary>
        [HtmlAttributeName("jmoz-interval")]
        public int Interval { get; set; }

        /// <summary>
        /// 发送的数据。
        /// </summary>
        [HtmlAttributeName("jmoz-data", DictionaryAttributePrefix = "jmoz-data-")]
        public IDictionary<string, string> Data { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 请求方法。
        /// </summary>
        [HtmlAttributeName("jmoz-method")]
        public AjaxMethod Method { get; set; }

        /// <summary>
        /// 缓存。
        /// </summary>
        [HtmlAttributeName("jmoz-cache")]
        public CacheType Cache { get; set; }

        /// <summary>
        /// 回掉方法。
        /// </summary>
        [HtmlAttributeName("jmoz-callback")]
        public string Callback { get; set; }

        private readonly ScriptWriter _writer = new ScriptWriter();
        private string _source;
        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var id = context.AllAttributes["id"]?.Value;
            if (id == null)
            {
                if (Cache != CacheType.None)
                    throw new Exception("缓存Id不能为空，必须设置Id属性！");
                id = "jmoz_" + context.UniqueId;
                output.Attributes.SetAttribute("id", id);
            }

            async Task<string> GetHtml()
            {
                var source = (await output.GetChildContentAsync())
                    .GetContent()?
                    .Trim();
                _writer.Write("<script>");
                _writer.Write("$(function ").Write("()").Write("{");
                _writer.Write($"var $this = $('#{id}');");
                JsRender(source);
                JsAjax();
                _writer.Write("});");
                _writer.Write("</script>");
                return _writer.ToString();
            }

            string html;
            switch (Cache)
            {
                case CacheType.None:
                    html = await GetHtml();
                    break;
                case CacheType.File:
                    html = await _cacheManager.GetOrCreateFileCacheAsync(ViewContext.ExecutingFilePath + id, 60,
                        key => GetHtml());
                    break;
                default:
                    html = await _cacheManager.GetOrCreateAsync(ViewContext.ExecutingFilePath + id, async ctx =>
                    {
                        ctx.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                        return await GetHtml();
                    });
                    break;
            }
            output.Content.AppendHtml(html);
        }

        private void SyntaxRender(ElementBase element)
        {
            switch (element.Type)
            {
                case ElementType.Text:
                    _writer.Html(element.Source);
                    return;
                case ElementType.Block:
                    if (element is CodeBlockElement block && _factory.TryGetSyntax(block.Key, out var syntax))
                    {
                        _writer.Write(syntax.Begin(block.Condition));
                        if (block.Any())
                        {
                            foreach (var child in block)
                            {
                                SyntaxRender(child);
                            }
                        }
                        _writer.Write(syntax.End());
                        return;
                    }
                    break;
            }
            _writer.Code(element.Source);
        }

        /// <summary>
        /// Render函数。
        /// </summary>
        private void JsRender(string source)
        {
            DocumentElement elements = null;
            _writer.Write("function _render(d){");
            if (string.IsNullOrWhiteSpace(source))
            {//直接呈现HTML代码
                _writer.Write("$this.html(d)");
            }
            else
            {
                _writer.Write("var _s = [];");
                _source = source;
                elements = new DocumentElement(_source);
                foreach (var element in elements)
                {
                    SyntaxRender(element);
                }
                _writer.Write("$this.html(_s.join(''));");
            }

            if (!string.IsNullOrWhiteSpace(Callback))
                _writer.Write(Callback).Write("(d);");

            if (Interval > 0)
            {//定时器
                _writer.Write("setTimeout(_ajax")
                    .Write(",")
                    .Write((Interval * 1000).ToString())
                    .Write(");");
            }

            _writer.Write("};");

            if (elements?.Scripts.Count > 0)
                _writer.Write(string.Join(";", elements.Scripts)).Write(";");
        }

        /// <summary>
        /// Ajax函数。
        /// </summary>
        private void JsAjax()
        {
            if (Method == AjaxMethod.GET && Data?.Count > 0)
            {
                if (Url.IndexOf('?') == -1)
                    Url += '?';
                else
                    Url += '&';
                Url += string.Join("&", Data.Select(x => $"{x.Key}={x.Value}"));
                Url = $"'{Url}&_'+(+new Date())";
            }
            _writer.Write("function _ajax(){");
            _writer.Write("$.ajax({url:")
                .Quote(Url)
                .Write(", type:")
                .Quote(Method.ToString())
                .Write(", dataType:'JSON'");
            if (Method == AjaxMethod.POST)
            {
                var data = Data == null ? "{}" : JsonConvert.SerializeObject(Data);
                _writer.Write(", data:").Write(data);
            }
            _writer.Write(", success:function(d){");
            _writer.Write("_render(d);");
            _writer.Write("}, error: function(r){");
            _writer.Write("$this.html(r.responseText);");

            if (Interval > 0)
            {//定时器
                _writer.Write("setTimeout(_ajax")
                    .Write(",")
                    .Write((Interval * 1000).ToString())
                    .Write(");");
            }
            _writer.Write("}});");
            _writer.Write("};_ajax();");
        }
    }
}