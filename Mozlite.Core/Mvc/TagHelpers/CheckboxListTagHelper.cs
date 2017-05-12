using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
/*
    相关样式：
    
.checkboxlist {
}

    .checkboxlist .checkbox {
        cursor: pointer;
        margin-top: 0;
        margin-bottom: 10px;
    }

        .checkboxlist .checkbox .box-wrapper {
            float: left;
            position: relative;
            display: inline-block;
            border: 1px solid #d2d2d2;
            padding: 0;
            padding-right: 16px;
            margin-bottom: 0;
            width: 16px;
            height: 15px;
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
            -webkit-background-clip: padding-box;
            -moz-background-clip: padding;
            background-clip: padding-box;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            -moz-transition: all 300ms ease-in-out;
            -webkit-transition: all 300ms ease-in-out;
            -o-transition: all 300ms ease-in-out;
            transition: all 300ms ease-in-out;
            -webkit-touch-callout: none;
            -webkit-user-select: none;
            -khtml-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

        .checkboxlist .checkbox input {
            position: absolute;
            display: block;
            left: 0;
            top: 0;
            right: 0;
            bottom: 0;
            margin: 0;
            visibility: hidden;
        }

        .checkboxlist .checkbox span {
            margin-left: 10px;
        }

        .checkboxlist .checkbox .box-wrapper .box-checked {
            position: absolute;
            display: block;
            left: 2px;
            right: 2px;
            bottom: 2px;
            top: 2px;
            -webkit-background-clip: padding-box;
            -moz-background-clip: padding;
            background-clip: padding-box;
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
            zoom: 1;
            -webkit-opacity: 0;
            -moz-opacity: 0;
            -ms-filter: progid:DXImageTransform.Microsoft.Alpha(Opacity=0);
            opacity: 0;
            filter: alpha(opacity=0);
            -moz-transition: 250ms all cubic-bezier(0.455, 0.030, 0.515, 0.955);
            -webkit-transition: 250ms all cubic-bezier(0.455, 0.030, 0.515, 0.955);
            -o-transition: 250ms all cubic-bezier(0.455, 0.030, 0.515, 0.955);
            transition: 250ms all cubic-bezier(0.455, 0.030, 0.515, 0.955);
        }

        .checkboxlist .checkbox:hover .box-wrapper,
        .checkboxlist .radio:hover .box-wrapper {
            -moz-box-shadow: 0 0 0 2px rgba(0,0,0,.03);
            -webkit-box-shadow: 0 0 0 2px rgba(0,0,0,.03);
            box-shadow: 0 0 0 2px rgba(0,0,0,.03);
        }

        .checkboxlist .checkbox.checked .box-wrapper .box-checked {
            opacity: 1;
        }

            .checkboxlist .checkbox.checked .box-wrapper .box-checked:before {
                font: normal normal normal 14px/1 FontAwesome;
                font-size: 12px;
                position: absolute;
            }

        .checkboxlist .checkbox.checked.checked-style-default .box-wrapper .box-checked {
            background: #d2d2d2;
        }

        .checkboxlist .checkbox.checked.checked-style-check .box-wrapper .box-checked:before {
            content: '\f00c';
        }
*/
namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 复选框列表标签。
    /// </summary>
    public abstract class CheckboxListTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// 名称。
        /// </summary>
        [HtmlAttributeName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 以“,”分割值。
        /// </summary>
        [HtmlAttributeName("value")]
        public string Value { get; set; }

        /// <summary>
        /// 每项样式类型。
        /// </summary>
        [HtmlAttributeName("iclass")]
        public string ItemClass { get; set; }

        /// <summary>
        /// 每项选中样式类型。
        /// </summary>
        [HtmlAttributeName("istyle")]
        public CheckedStyle CheckedStyle { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Value != null)
                Value = $",{Value},";
            var items = new Dictionary<string, string>();
            Init(items);
            foreach (var item in items)
            {
                output.Content.AppendHtml(Create(item.Key, item.Value, IsChecked(item.Value)));
            }
            output.TagName = "div";
            output.MergeClassNames(context, "checkboxlist");
        }

        /// <summary>
        /// 判断选中的状态。
        /// </summary>
        /// <param name="value">当前列表值。</param>
        /// <param name="current">当前项目值。</param>
        /// <returns>返回判断结果。</returns>
        protected virtual bool IsChecked(string current)
        {
            return Value?.IndexOf($",{current},") >= 0;
        }

        /// <summary>
        /// 附加复选项目列表，文本/值。
        /// </summary>
        /// <param name="items">复选框项目列表实例。</param>
        protected abstract void Init(IDictionary<string, string> items);

        private TagBuilder Create(string text, string value, bool isChecked)
        {
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("checkbox");
            if (ItemClass != null)
                wrapper.AddCssClass(ItemClass);
            if (isChecked)
                wrapper.AddCssClass("checked");
            wrapper.AddCssClass("checked-style-" + CheckedStyle.ToString().ToLower());
            wrapper.MergeAttribute("onclick", "$(this).toggleClass('checked');if($(this).hasClass('checked')){$(this).find('input')[0].checked='checked';}else{$(this).find('input').removeAttr('checked');}");

            var input = new TagBuilder("input");
            input.MergeAttribute("type", "checkbox");
            input.MergeAttribute("name", Name);
            input.MergeAttribute("value", value);
            if (isChecked)
                input.MergeAttribute("checked", "checked");
            wrapper.InnerHtml.AppendHtml(input);

            var label = new TagBuilder("label");
            label.AddCssClass("box-wrapper");
            label.InnerHtml.AppendHtml("<div class=\"box-checked\"></div>");
            wrapper.InnerHtml.AppendHtml(label);

            var span = new TagBuilder("span");
            span.InnerHtml.AppendHtml(text);
            wrapper.InnerHtml.AppendHtml(span);
            return wrapper;
        }
    }

    /// <summary>
    /// 选中样式。
    /// </summary>
    public enum CheckedStyle
    {
        /// <summary>
        /// 默认。
        /// </summary>
        Default,

        /// <summary>
        /// 打勾。
        /// </summary>
        Check,
    }
}