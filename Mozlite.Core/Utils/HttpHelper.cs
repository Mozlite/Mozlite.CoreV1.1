using Mozlite.FileProviders;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Mozlite.Utils
{
    /// <summary>
    /// HTTP辅助类。
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 用户代理。
        /// </summary>
        public const string UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

        #region gets
        /// <summary>
        /// 获取HTML代码。
        /// </summary>
        /// <param name="url">URL地址。</param>
        /// <param name="referenceUrl">引用地址。</param>
        /// <returns>返回HTML代码。</returns>
        public static async Task<string> GetHtmlAsync(string url, string referenceUrl = null)
        {
            return await GetHtmlAsync(new Uri(url), referenceUrl);
        }

        /// <summary>
        /// 获取HTML代码。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="referenceUrl">引用地址。</param>
        /// <returns>返回HTML代码。</returns>
        public static async Task<string> GetHtmlAsync(Uri uri, string referenceUrl = null)
        {
            return await ExecuteAsync(async client => await client.GetStringAsync(uri), referenceUrl);
        }

        /// <summary>
        /// 获取当前URL路径的文件流。
        /// </summary>
        /// <param name="url">URL地址。</param>
        /// <param name="referenceUrl">引用地址。</param>
        /// <returns>返回文件流。</returns>
        public static async Task<Stream> GetStreamAsync(string url, string referenceUrl = null)
        {
            return await GetStreamAsync(new Uri(url), referenceUrl);
        }

        /// <summary>
        /// 获取当前URL路径的文件流。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="referenceUrl">引用地址。</param>
        /// <returns>返回文件流。</returns>
        public static async Task<Stream> GetStreamAsync(Uri uri, string referenceUrl = null)
        {
            return await ExecuteAsync(async client => await client.GetStreamAsync(uri), referenceUrl);
        }
        #endregion

        #region posts
        /// <summary>
        /// 获取HTML代码。
        /// </summary>
        /// <param name="url">URL地址。</param>
        /// <param name="referenceUrl">引用地址。</param>
        /// <param name="content">发送内容。</param>
        /// <returns>返回HTML代码。</returns>
        public static async Task<string> PostHtmlAsync(string url, string referenceUrl = null, HttpContent content = null)
        {
            return await PostHtmlAsync(new Uri(url), referenceUrl, content);
        }

        /// <summary>
        /// 获取HTML代码。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="referenceUrl">引用地址。</param>
        /// <param name="content">发送内容。</param>
        /// <param name="headers">配置头部信息。</param>
        /// <returns>返回HTML代码。</returns>
        public static async Task<string> PostHtmlAsync(Uri uri, string referenceUrl = null, HttpContent content = null, Action<HttpRequestHeaders> headers = null)
        {
            return await ExecuteAsync(async client =>
            {
                headers?.Invoke(client.DefaultRequestHeaders);
                var response = await client.PostAsync(uri, content);
                var message = response.EnsureSuccessStatusCode();
                return await message.Content.ReadAsStringAsync();
            }, referenceUrl);
        }

        /// <summary>
        /// 获取当前URL路径的文件流。
        /// </summary>
        /// <param name="url">URL地址。</param>
        /// <param name="referenceUrl">引用地址。</param>
        /// <param name="content">发送内容。</param>
        /// <returns>返回响应内容。</returns>
        public static async Task<Stream> PostAsync(string url, string referenceUrl = null, HttpContent content = null)
        {
            return await PostAsync(new Uri(url), referenceUrl, content);
        }

        /// <summary>
        /// 获取当前URL路径的文件流。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="referenceUrl">引用地址。</param>
        /// <param name="content">发送内容。</param>
        /// <param name="headers">配置头部信息。</param>
        /// <returns>返回响应内容。</returns>
        public static async Task<Stream> PostAsync(Uri uri, string referenceUrl = null, HttpContent content = null, Action<HttpRequestHeaders> headers = null)
        {
            return await ExecuteAsync(async client =>
            {
                headers?.Invoke(client.DefaultRequestHeaders);
                var response = await client.PostAsync(uri, content);
                var message = response.EnsureSuccessStatusCode();
                return await message.Content.ReadAsStreamAsync();
            }, referenceUrl);
        }
        #endregion

        #region download
        /// <summary>
        /// 下载文件并保存到目录中，如果文件已经存在则不下载。
        /// </summary>
        /// <param name="url">URL地址。</param>
        /// <param name="dir">保存的文件夹物理路径。</param>
        /// <param name="fileNameWithoutExtension">不包含扩展名的文件名。</param>
        /// <returns>返回文件名称。</returns>
        public static async Task<string> DownloadAsync(string url, string dir, string fileNameWithoutExtension)
        {
            var uri = new Uri(url);
            var name = fileNameWithoutExtension == null ?
                Path.GetFileName(uri.AbsolutePath) :
                fileNameWithoutExtension + Path.GetExtension(uri.AbsolutePath);
            var path = Path.Combine(dir, name);
            if (File.Exists(path))
                return name;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return await ExecuteAsync(async client =>
            {
                using (var stream = await client.GetStreamAsync(uri))
                {
                    await stream.SaveToAsync(path);
                }
                return name;
            });
        }

        /// <summary>
        /// 下载文件并保存到目录中，如果文件已经存在则不下载。
        /// </summary>
        /// <param name="url">URL地址。</param>
        /// <param name="path">保存文件（临时文件）物理路径。</param>
        /// <returns>返回文件扩展名称。</returns>
        public static async Task<string> DownloadAsync(string url, string path)
        {
            try
            {
                var uri = new Uri(url);
                return await ExecuteAsync(async client =>
                {
                    using (var stream = await client.GetStreamAsync(uri))
                    {
                        await stream.SaveToAsync(path);
                    }
                    return Path.GetExtension(uri.AbsolutePath);
                });
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region helper
        private static async Task<T> ExecuteAsync<T>(Func<HttpClient, Task<T>> func, string referenceUrl = null)
        {
            using (var client = new HttpClient())
            {
                if (referenceUrl != null)
                    client.DefaultRequestHeaders.Referrer = new Uri(referenceUrl);
                client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                return await func(client);
            }
        }

        private static T Execute<T>(Func<HttpClient, T> func, string referenceUrl = null)
        {
            using (var client = new HttpClient())
            {
                if (referenceUrl != null)
                    client.DefaultRequestHeaders.Referrer = new Uri(referenceUrl);
                client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                return func(client);
            }
        }

        /// <summary>
        /// 获取URL的首页域名地址。
        /// </summary>
        /// <param name="uri">当前URL地址。</param>
        /// <returns>返回URL的首页域名地址。</returns>
        public static string GetHomeUrl(this Uri uri)
        {
            return $"{uri.Scheme}://{uri.DnsSafeHost}{(uri.IsDefaultPort ? null : ":" + uri.Port)}/";
        }

        /// <summary>
        /// 获取URL的首页域名地址。
        /// </summary>
        /// <param name="url">当前URL地址。</param>
        /// <returns>返回URL的首页域名地址。</returns>
        public static string GetHomeUrl(this string url)
        {
            var uri = new Uri(url);
            return GetHomeUrl(uri);
        }

        /// <summary>
        /// 判断是否为本地地址。
        /// </summary>
        /// <param name="url">URL地址。</param>
        /// <returns>返回判断结果。</returns>
        public static bool IsLocalUrl(this string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            if (url[0] == 46)//../
                return true;
            if (url[0] == 47 && (url.Length == 1 || url[1] != 47 && url[1] != 92))//
                return true;
            if (url.Length > 1 && url[0] == 126)//~/
                return url[1] == 47;
            return false;
        }
        #endregion
    }
}