using Mozlite.Core;
using Mozlite.Extensions.Categories;

namespace Mozlite.Extensions.Channels
{
    /// <summary>
    /// 频道管理接口。
    /// </summary>
    public interface IChannelManager : ICategoryManager<Channel>, ISingletonService
    {

    }
}