using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Mozlite.FileProviders
{
    /// <summary>
    /// 文件操作辅助类。
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IOHelper
    {
        /// <summary>
        /// 读取所有文件内容。
        /// </summary>
        /// <param name="path">文件的物理路径。</param>
        /// <param name="share">文件共享选项。</param>
        /// <returns>返回文件内容字符串。</returns>
        public static string ReadText(string path, FileShare share = FileShare.None)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, share))
            {
                using (var reader = new StreamReader(fs, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 读取所有文件内容。
        /// </summary>
        /// <param name="path">文件的物理路径。</param>
        /// <param name="share">文件共享选项。</param>
        /// <returns>返回文件内容字符串。</returns>
        public static async Task<string> ReadTextAsync(string path, FileShare share = FileShare.None)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, share))
            {
                using (var reader = new StreamReader(fs, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        /// <summary>
        /// 保存文件内容。
        /// </summary>
        /// <param name="path">文件的物理路径。</param>
        /// <param name="text"></param>
        /// <param name="share">文件共享选项。</param>
        /// <returns>返回写入任务实例对象。</returns>
        public static void SaveText(string path, string text, FileShare share = FileShare.None)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, share))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(text);
                }
            }
        }

        /// <summary>
        /// 保存文件内容。
        /// </summary>
        /// <param name="path">文件的物理路径。</param>
        /// <param name="text"></param>
        /// <param name="share">文件共享选项。</param>
        /// <returns>返回写入任务实例对象。</returns>
        public static async Task SaveTextAsync(string path, string text, FileShare share = FileShare.None)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, share))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    await sw.WriteAsync(text);
                }
            }
        }

        /// <summary>
        /// 将文件流保存到文件中。
        /// </summary>
        /// <param name="stream">当前文件流。</param>
        /// <param name="path">文件的物理路径。</param>
        /// <param name="share">文件共享选项。</param>
        /// <returns>返回保存任务。</returns>
        public static async Task SaveToAsync(this Stream stream, string path, FileShare share = FileShare.None)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, share))
            {
                var size = 409600;
                var buffer = new byte[size];
                var current = await stream.ReadAsync(buffer, 0, size);
                while (current > 0)
                {
                    await fs.WriteAsync(buffer, 0, current);
                    current = await stream.ReadAsync(buffer, 0, size);
                }
            }
        }

        /// <summary>
        /// 获取文件的物理路径。
        /// </summary>
        /// <param name="env">环境实例对象。</param>
        /// <param name="baseWebRootPath">基于WebRoot文件夹的文件相对路径。</param>
        /// <returns>返回当前文件的物理路径。</returns>
        public static string MapPath(this IHostingEnvironment env, string baseWebRootPath)
        {
            return Path.Combine(env.WebRootPath, baseWebRootPath);
        }
    }
}