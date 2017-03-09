using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mozlite.Properties;

namespace Mozlite.FileProviders
{
    /// <summary>
    /// 媒体控制器。
    /// </summary>
    public class MediaController : Mvc.Controllers.ControllerBase
    {
        private readonly IMediaFileProvider _provider;

        /// <summary>
        /// 初始化类<see cref="MediaController"/>。
        /// </summary>
        /// <param name="provider">媒体文件提供者。</param>
        public MediaController(IMediaFileProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 访问媒体文件。
        /// </summary>
        /// <param name="type">媒体类型。</param>
        /// <param name="name">文件名称。</param>
        /// <returns>返回文件结果。</returns>
        [Route("media/{type}/{name}")]
        public IActionResult Index(string type, string name)
        {
            var mediaId = Path.GetFileNameWithoutExtension(name).AsGuid();
            if (mediaId == null)
                return NotFound();
            var storageFile = _provider.GetStorageFile(mediaId.Value);
            if (storageFile == null)
                return NotFound();
            return File(storageFile.CreateOpenReadStream(), storageFile.ContentType);
        }

        /// <summary>
        /// 浏览媒体文件。
        /// </summary>
        /// <param name="type">媒体类型。</param>
        /// <param name="id">所属对象Id。</param>
        /// <param name="dir">目录Id。</param>
        /// <returns>返回文件结果。</returns>
        [Authorize]
        [Route("browser/{type}/{id}")]
        [Route("browser/{type}/{id}-{dir}")]
        public IActionResult Browser(string type, int id, int? dir = null)
        {
            ViewBag.Title = Resources.BrowserTitle;
            if (type == null || id <= 0)
                return View();
            return View(_provider.LoadByTargetId(type, id, dir));
        }


        /// <summary>
        /// 浏览媒体文件。
        /// </summary>
        /// <param name="file">上传的文件。</param>
        /// <param name="type">媒体类型。</param>
        /// <param name="id">所属对象Id。</param>
        /// <param name="dir">目录Id。</param>
        /// <returns>返回文件结果。</returns>
        [HttpPost]
        [Authorize]
        [Route("browser/{type}/{id}")]
        [Route("browser/{type}/{id}-{dir}")]
        public async Task<IActionResult> Browser(IFormFile file, string type, int id, int? dir = null)
        {
            ViewBag.Title = Resources.BrowserTitle;
            if (file == null)
                return Error(Resources.PleaseSelectFirstAndThenUpload);
            if (type == null || id <= 0)
                return Error(Resources.MediaFileUploadPathIsError);
            var url = await _provider.UploadAsync(file, type, id, dir ?? 0);
            if (url == null)
                return Error(Resources.UploadFailured);
            return Success(Resources.UploadSuccess, new { url });
        }
    }
}