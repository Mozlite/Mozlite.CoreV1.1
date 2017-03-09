using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Mozlite.Core;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 枚举下拉列表标签。
    /// </summary>
    [HtmlTargetElement("x:enumselect")]
    public class EnumSelectTagHelper : SelectableTagHelperBase
    {
        private readonly IEnumLocalizer _localizer;

        /// <summary>
        /// 当前枚举类型。
        /// </summary>
        [HtmlAttributeName("x-type")]
        public Type Type { get; set; }

        /// <summary>
        /// 初始化类<see cref="EnumSelectTagHelper"/>。
        /// </summary>
        /// <param name="generator">Html辅助接口。</param>
        /// <param name="localizer">本地化实例。</param>
        public EnumSelectTagHelper(IHtmlGenerator generator, IEnumLocalizer localizer)
            :base(generator)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// 添加下拉列表框选项。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="items">下拉列表框列表。</param>
        protected override void Init(List<SelectListItem> items, TagHelperContext context)
        {
            var type = For?.ModelExplorer.ModelType ?? Type;
            if (type != null)
            {
                type = type.UnwrapNullableType();
                foreach (Enum value in Enum.GetValues(type))
                {
                    items.Add(new SelectListItem { Text = _localizer.L(value)??value.ToString(), Value = value.ToString() });
                }
            }
        }
    }
}