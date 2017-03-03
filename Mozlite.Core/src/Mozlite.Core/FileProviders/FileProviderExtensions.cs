using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mozlite.Utils;

namespace Mozlite.FileProviders
{
    /// <summary>
    /// 扩展类。
    /// </summary>
    public static class FileProviderExtensions
    {
        /// <summary>
        /// 判断是否为本地媒体文件路径。
        /// </summary>
        /// <param name="url">当前媒体路径。</param>
        /// <returns>返回判断结果。</returns>
        public static bool IsLocalMediaUrl(this string url)
        {
            if (!url.IsLocalUrl())
                return false;
            return Path.GetFileNameWithoutExtension(url).AsGuid() != null;
        }

        /// <summary>
        /// 保存到物理文件路径中。
        /// </summary>
        /// <param name="file">表单文件实例对象。</param>
        /// <param name="path">物理路径。</param>
        /// <returns>返回保存结果。</returns>
        public static async Task<bool> SaveToAsync(this IFormFile file, string path)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fs);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="provider">媒体文件提供者。</param>
        /// <param name="file">上传的文件实例。</param>
        /// <param name="mediaType">媒体类型。</param>
        /// <param name="targetId">所属实例的Id。</param>
        /// <param name="directoryId">所属目录的Id。</param>
        /// <returns>返回文件访问的URL地址。</returns>
        public static Task<string> UploadAsync(this IMediaFileProvider provider, IFormFile file, string mediaType, int targetId = 0, int directoryId = 0)
        {
            return provider.UploadAsync(file, false, mediaType, targetId, directoryId);
        }
    }
}