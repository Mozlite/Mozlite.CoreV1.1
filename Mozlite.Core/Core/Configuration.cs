﻿using Microsoft.Extensions.Configuration;

namespace Mozlite.Core
{
    /// <summary>
    /// 配置实例。
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// 默认后台操作文件作路径名称。
        /// </summary>
        public const string DefaultBackendDir = "backend";

        /// <summary>
        /// 默认用户中心操作路径名称。
        /// </summary>
        public const string DefaultUserCenterDir = "self";

        private readonly IConfiguration _configuration;

        internal Configuration(IConfiguration configuration)
        {
            _configuration = configuration;
            var section = configuration.GetSection("routes");
            Backend = section["backend"] ?? DefaultBackendDir;
            UserCenter = section["usercenter"] ?? DefaultUserCenterDir;
            StorageDir = configuration["storagedir"];
        }

        /// <summary>
        /// 存储文件路径。
        /// </summary>
        public string StorageDir { get; }

        /// <summary>
        /// 获取配置节点。
        /// </summary>
        /// <param name="key">配置键。</param>
        /// <returns>返回配置节点实例。</returns>
        public IConfigurationSection GetSection(string key) => _configuration.GetSection(key);

        /// <summary>
        /// 后台操作文件作路径名称。
        /// </summary>
        public string Backend { get; }

        /// <summary>
        /// 用户中心操作路径名称。
        /// </summary>
        public string UserCenter { get; }
    }
}