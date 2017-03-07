using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Mozlite.Extensions.Channels;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 频道下拉列表框。
    /// </summary>
    [HtmlTargetElement("x:channelselect")]
    public class ChannelSelectListTagHelper : SelectableTagHelper
    {
        private readonly IChannelManager _channelManager;
        /// <summary>
        /// 初始化类<see cref="ChannelSelectListTagHelper"/>。
        /// </summary>
        /// <param name="generator">Html辅助接口。</param>
        /// <param name="channelManager">频道管理接口。</param>
        public ChannelSelectListTagHelper(IHtmlGenerator generator, IChannelManager channelManager) : base(generator)
        {
            _channelManager = channelManager;
        }

        /// <summary>
        /// 添加下拉列表框选项。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="items">下拉列表框列表。</param>
        protected override void Init(List<SelectListItem> items, TagHelperContext context)
        {
            var channels = _channelManager.LoadCaches();
            var id = context.AllAttributes["value"]?.Value.ToString().AsInt32();
            foreach (var channel in channels)
            {
                items.Add(new SelectListItem { Text = channel.Name, Value = channel.Id.ToString(), Selected = id == channel.Id });
            }
        }
    }
}