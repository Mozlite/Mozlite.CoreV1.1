using System;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.PlatformAbstractions;

namespace Mozlite
{
    /// <summary>
    /// 核心信息。
    /// </summary>
    public class CoreInfo
    {
        private static readonly CoreInfo _info = new CoreInfo();
        private CoreInfo()
        {
            Version = Assembly.GetEntryAssembly().GetName().Version;
        }

        /// <summary>
        /// 获取环境变量。
        /// </summary>
        public static CoreInfo Default => _info;

        /// <summary>
        /// 版本。
        /// </summary>
        public Version Version { get; }
    }
}