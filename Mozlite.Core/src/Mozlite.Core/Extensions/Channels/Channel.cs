using Mozlite.Data.Metadata;
using Mozlite.Extensions.Categories;

namespace Mozlite.Extensions.Channels
{
    /// <summary>
    /// 频道，网站的最大信息分类，一般显示在网站导航上。
    /// </summary>
    [Table("Channels")]
    public class Channel : CategoryBase
    {
        /// <summary>
        /// 图标名称。
        /// </summary>
        [Size(20)]
        public string IconName { get; set; }

        /// <summary>
        /// 样式名称。
        /// </summary>
        [Size(20)]
        public string ClassName { get; set; }

        /// <summary>
        /// 显示名称。
        /// </summary>
        [Size(20)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 链接地址。
        /// </summary>
        [Size(256)]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 优先级。
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 链接目标。
        /// </summary>
        [Size(20)]
        public string LinkTarget { get; set; }

        /// <summary>
        /// 禁用不显示。
        /// </summary>
        public bool Disabled { get; set; }
    }
}