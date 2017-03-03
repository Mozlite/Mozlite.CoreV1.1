using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Mozlite.Data;
using Mozlite.Extensions.Categories;

namespace Mozlite.Extensions.Channels
{
    /// <summary>
    /// 频道管理实现类。
    /// </summary>
    public class ChannelManager : CategoryManager<Channel>, IChannelManager
    {
        /// <summary>
        /// 初始化类<see cref="ChannelManager"/>。
        /// </summary>
        /// <param name="repository">数据库操作接口。</param>
        /// <param name="cache">缓存接口。</param>
        public ChannelManager(IRepository<Channel> repository, IMemoryCache cache) : base(repository, cache)
        {
        }

        /// <summary>
        /// 加载所有分类。
        /// </summary>
        /// <returns>分类列表。</returns>
        public override IEnumerable<Channel> Load()
        {
            return base.Load().OrderBy(x => x.Priority);
        }
    }
}