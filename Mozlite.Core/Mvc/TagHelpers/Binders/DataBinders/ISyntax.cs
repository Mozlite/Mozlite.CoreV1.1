using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Mozlite.Core;

namespace Mozlite.Mvc.TagHelpers.Binders.DataBinders
{
    /// <summary>
    /// 语法接口。
    /// </summary>
    public interface ISyntax : IServices
    {
        /// <summary>
        /// 关键字。
        /// </summary>
        string Keyword { get; }

        /// <summary>
        /// 生成语句。
        /// </summary>
        /// <param name="output">输出标签实例。</param>
        /// <param name="code">条件代码块。</param>
        /// <param name="instance">当前实例对象。</param>
        /// <param name="element">当前节点实例。</param>
        /// <returns>返回生成的语句。</returns>
        void Parse(TagHelperOutput output, string code, object instance, CodeBlockElement element);
    }

    public abstract class Syntax : ISyntax
    {
        /// <summary>
        /// 呈现对象节点。
        /// </summary>
        protected internal Action<TagHelperOutput, object, Element> Render { get; internal set; }

        /// <summary>
        /// 获取对象的属性值。
        /// </summary>
        protected internal Func<object, string, string> GetValue { get; internal set; }

        /// <summary>
        /// 关键字。
        /// </summary>
        public abstract string Keyword { get; }

        /// <summary>
        /// 生成语句。
        /// </summary>
        /// <param name="output">输出标签实例。</param>
        /// <param name="code">条件代码块。</param>
        /// <param name="instance">当前实例对象。</param>
        /// <param name="element">当前节点实例。</param>
        /// <returns>返回生成的语句。</returns>
        public abstract void Parse(TagHelperOutput output, string code, object instance, CodeBlockElement element);
    }
}