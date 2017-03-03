using System.Threading.Tasks;
using Mozlite.Core;

namespace Mozlite.Mvc.Tools
{
    /// <summary>
    /// 样式文件管理类，主要用于开发时候仿照别人网站下载图片文件。
    /// </summary>
    public interface IStyleManager : ISingletonService
    {
        /// <summary>
        /// 下载远程图片文件并替换样式文件。
        /// </summary>
        /// <param name="fileName">当前样式文件</param>
        /// <param name="dirName">保存图片目录名称。</param>
        /// <returns>返回执行结果。</returns>
        Task<bool> ParseAsync(string fileName, string dirName);
    }
}