using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Mozlite.Core.Tasks;

namespace Mozlite.Mvc.TagHelpers
{
    /// <summary>
    /// 后台服务下拉列表标签。
    /// </summary>
    [HtmlTargetElement("x:taskselect")]
    public class TaskSelectTagHelper : SelectableTagHelper
    {
        private readonly ITaskManager _taskManager;

        /// <summary>
        /// 初始化类<see cref="SelectableTagHelper"/>。
        /// </summary>
        /// <param name="generator">Html辅助接口。</param>
        /// <param name="taskManager">服务管理接口。</param>
        public TaskSelectTagHelper(IHtmlGenerator generator, ITaskManager taskManager) : base(generator)
        {
            _taskManager = taskManager;
        }

        /// <summary>
        /// 扩展类型。
        /// </summary>
        [HtmlAttributeName("x-extension")]
        public string ExtensionName { get; set; }

        /// <summary>
        /// 添加下拉列表框选项。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="items">下拉列表框列表。</param>
        protected override void Init(List<SelectListItem> items, TagHelperContext context)
        {
            var tasks = _taskManager.LoadArgumentTasks(ExtensionName);
            foreach (var task in tasks)
            {
                items.Add(new SelectListItem { Text = task.Name, Value = task.Id.ToString() });
            }
        }
    }
}