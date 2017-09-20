using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Configuration;
using System.Runtime.InteropServices;

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
            FrameworkName = RuntimeInformation.FrameworkDescription;
            OSName = RuntimeInformation.OSDescription;
        }

        /// <summary>
        /// 获取环境变量。
        /// </summary>
        public static CoreInfo Default { get; } = new CoreInfo();

        /// <summary>
        /// 版本。
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// 框架版本。
        /// </summary>
        public string FrameworkName { get; }

        /// <summary>
        /// 操作系统。
        /// </summary>
        public string OSName { get; set; }
    }
}