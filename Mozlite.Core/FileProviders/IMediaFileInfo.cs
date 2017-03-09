using Microsoft.Extensions.FileProviders;

namespace Mozlite.FileProviders
{
    /// <summary>
    /// 媒体文件信息接口。
    /// </summary>
    public interface IMediaFileInfo : IFileInfo
    {
        /// <summary>
        /// 获取Md5值。
        /// </summary>
        string Md5 { get; }

        /// <summary>
        /// 访问的URL地址。
        /// </summary>
        string Url { get; }
    }
}