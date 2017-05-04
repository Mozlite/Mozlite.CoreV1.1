using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Mozlite.Core.Caching
{
    /// <summary>
    /// 缓存管理接口。
    /// </summary>
    public interface ICacheManager : IMemoryCache, ISingletonService
    {
        /// <summary>
        /// 获取或创建文件缓存。
        /// </summary>
        /// <param name="key">缓存键。</param>
        /// <param name="minutes">缓存时间，如果最后访问时间和现在时间差距超过<paramref name="minutes"/>则删除缓存文件。</param>
        /// <param name="func">缓存实例。</param>
        /// <returns>返回当前缓存的内容。</returns>
        string GetOrCreateFileCache(object key, int minutes, Func<object, string> func);

        /// <summary>
        /// 获取或创建文件缓存。
        /// </summary>
        /// <param name="key">缓存键。</param>
        /// <param name="minutes">缓存时间，如果最后访问时间和现在时间差距超过<paramref name="minutes"/>则删除缓存文件。</param>
        /// <param name="func">缓存实例。</param>
        /// <returns>返回当前缓存的内容。</returns>
        Task<string> GetOrCreateFileCacheAsync(object key, int minutes, Func<object, Task<string>> func);
    }
}