using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Mozlite.FileProviders;

namespace Mozlite.Core.Caching
{
    /// <summary>
    /// 换成管理实现类。
    /// </summary>
    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _cache;
        private readonly IMediaFileProvider _fileProvider;

        /// <summary>
        /// 初始化类<see cref="CacheManager"/>。
        /// </summary>
        /// <param name="cache">缓存接口。</param>
        /// <param name="fileProvider">文件提供者。</param>
        public CacheManager(IMemoryCache cache, IMediaFileProvider fileProvider)
        {
            _cache = cache;
            _fileProvider = fileProvider;
        }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose() => _cache.Dispose();

        /// <summary>Gets the item associated with this key if present.</summary>
        /// <param name="key">An object identifying the requested entry.</param>
        /// <param name="value">The located value or null.</param>
        /// <returns>True if the key was found.</returns>
        public bool TryGetValue(object key, out object value) => _cache.TryGetValue(key, out value);

        /// <summary>Create or overwrite an entry in the cache.</summary>
        /// <param name="key">An object identifying the entry.</param>
        /// <returns>The newly created <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> instance.</returns>
        public ICacheEntry CreateEntry(object key) => _cache.CreateEntry(key);

        /// <summary>Removes the object associated with the given key.</summary>
        /// <param name="key">An object identifying the entry.</param>
        public void Remove(object key) => _cache.Remove(key);

        /// <summary>
        /// 获取或创建文件缓存。
        /// </summary>
        /// <param name="key">缓存键。</param>
        /// <param name="minutes">缓存时间，如果最后访问时间和现在时间差距超过<paramref name="minutes"/>则删除缓存文件。</param>
        /// <param name="func">缓存实例。</param>
        /// <returns>返回当前缓存的内容。</returns>
        public string GetOrCreateFileCache(object key, int minutes, Func<object, string> func)
        {
            return _fileProvider.GetOrCreateFileCache(key, minutes, func);
        }

        /// <summary>
        /// 获取或创建文件缓存。
        /// </summary>
        /// <param name="key">缓存键。</param>
        /// <param name="minutes">缓存时间，如果最后访问时间和现在时间差距超过<paramref name="minutes"/>则删除缓存文件。</param>
        /// <param name="func">缓存实例。</param>
        /// <returns>返回当前缓存的内容。</returns>
        public Task<string> GetOrCreateFileCacheAsync(object key, int minutes, Func<object, Task<string>> func)
        {
            return _fileProvider.GetOrCreateFileCacheAsync(key, minutes, func);
        }
    }
}