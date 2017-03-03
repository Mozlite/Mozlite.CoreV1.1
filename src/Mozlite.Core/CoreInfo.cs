using System;
using System.Reflection;

namespace Mozlite
{
    /// <summary>
    /// 核心信息。
    /// </summary>
    public class CoreInfo
    {
        private CoreInfo()
        {
            Version = Assembly.GetEntryAssembly().GetName().Version;
        }

        /// <summary>
        /// 获取环境变量。
        /// </summary>
        public static CoreInfo Default { get; } = new CoreInfo();

        /// <summary>
        /// 版本。
        /// </summary>
        public Version Version { get; }
    }
}