#if DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Mozlite.Utils;
using Mozlite.FileProviders;

namespace Mozlite.Mvc.Tools
{
    /// <summary>
    /// 样式文件里图片下载器，仿制别人的网站时候使用。
    /// </summary>
    public class StyleManager : IStyleManager
    {
        private readonly IHostingEnvironment _env;
        private static readonly Regex _regex = new Regex("url\\((.+?)\\)", RegexOptions.IgnoreCase);
        public StyleManager(IHostingEnvironment env)
        {
            _env = env;
        }

        private IEnumerable<string> GetImages(string text)
        {
            var list = new List<string>();
            foreach (Match match in _regex.Matches(text))
            {
                var path = match.Groups[1].Value;
                path = path.Trim(' ', '\'', '"');
                if (path.IsLocalUrl() || path.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                    continue;
                list.Add(path);
            }
            return list.Distinct(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 下载远程图片文件并替换样式文件。
        /// </summary>
        /// <param name="fileName">当前样式文件</param>
        /// <param name="dirName">保存图片目录名称。</param>
        /// <returns>返回执行结果。</returns>
        public async Task<bool> ParseAsync(string fileName, string dirName)
        {
            fileName = Path.Combine(_env.WebRootPath, "css", fileName);
            var text = await IOHelper.ReadTextAsync(fileName);
            if (string.IsNullOrWhiteSpace(text))
                return true;
            var images = GetImages(text);
            var dir = Path.Combine(_env.WebRootPath, "images", dirName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var missings = new List<string>();
            foreach (var image in images)
            {
                try
                {
                    var url = image;
                    if (url.StartsWith("//"))
                        url = "http:" + url;
                    var name = await HttpHelper.DownloadAsync(url, dir);
                    text = text.Replace(image, $"/images/{dirName}/{name}");
                }
                catch
                {
                    missings.Add(image);
                }
            }
            if (File.Exists(fileName + ".src"))
                File.Delete(fileName + ".src");
            File.Move(fileName, fileName + ".src");
            if (missings.Count > 0)
                text = $"/*Style downloader 404 response:\r\n{string.Join("\r\n", missings)}*/\r\n{text}";
            await IOHelper.SaveTextAsync(fileName, text);
            return true;
        }
    }
}
#endif