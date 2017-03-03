using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mozlite.Core;

namespace Mozlite.FileProviders
{
    /// <summary>
    /// 媒体文件夹文件提供者接口。
    /// </summary>
    public interface IMediaFileProvider : ISingletonService
    {
        /// <summary>
        /// 获取子路径下的文件信息。
        /// </summary>
        /// <param name="mediaId">文件ID。</param>
        /// <returns>返回媒体文件信息。</returns>
        IMediaFileInfo GetFileInfo(Guid mediaId);

        /// <summary>
        /// 获取实体媒体文件实例。
        /// </summary>
        /// <param name="mediaId">媒体文件ID。</param>
        /// <returns>返回实体媒体文件实例。</returns>
        StorageFile GetStorageFile(Guid mediaId);

        /// <summary>
        /// 设置标题。
        /// </summary>
        /// <param name="id">媒体文件Id。</param>
        /// <param name="title">标题。</param>
        /// <returns>返回设置结果。</returns>
        bool SetTitle(Guid id, string title);

        /// <summary>
        /// 将文件移动到相应的目录中。
        /// </summary>
        /// <param name="id">媒体文件Id。</param>
        /// <param name="directoryId">目录Id。</param>
        /// <returns>返回执行结果。</returns>
        bool MoveTo(Guid id, int directoryId);

        /// <summary>
        /// 加载当前目录下的文件。
        /// </summary>
        /// <param name="mediaType">媒体类型。</param>
        /// <param name="directoryId">目录Id。</param>
        /// <param name="targetId">所属实例Id。</param>
        /// <returns>返回媒体文件列表。</returns>
        IEnumerable<MediaFileInfo> LoadByDirectoryId(string mediaType, int directoryId, int targetId = 0);

        /// <summary>
        /// 加载当前实例下的文件。
        /// </summary>
        /// <param name="mediaType">媒体类型。</param>
        /// <param name="targetId">所属实例Id。</param>
        /// <param name="directoryId">目录Id。</param>
        /// <returns>返回媒体文件列表。</returns>
        IEnumerable<MediaFileInfo> LoadByTargetId(string mediaType, int targetId, int? directoryId = null);

        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="url">URL地址。</param>
        /// <param name="mediaType">媒体类型。</param>
        /// <param name="isUnique">文件名称和实体文件名称是否一一对应。</param>
        /// <param name="targetId">所属实例的Id。</param>
        /// <param name="directoryId">所属目录的Id。</param>
        /// <param name="extension">扩展名称，主要为了有些图片地址无扩展名而使用。</param>
        /// <returns>返回文件访问的URL地址。</returns>
        Task<string> DownloadAsync(string url, bool isUnique, string mediaType, int targetId = 0, int directoryId = 0, string extension = null);

        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="file">上传的文件实例。</param>
        /// <param name="isUnique">文件名称和实体文件名称是否一一对应。</param>
        /// <param name="mediaType">媒体类型。</param>
        /// <param name="targetId">所属实例的Id。</param>
        /// <param name="directoryId">所属目录的Id。</param>
        /// <returns>返回文件访问的URL地址。</returns>
        Task<string> UploadAsync(IFormFile file, bool isUnique, string mediaType, int targetId = 0, int directoryId = 0);

        /// <summary>
        /// 上传文件替换掉原有的媒体文件的实体文件，如果文件名不是GUID将生成新的文件名。
        /// </summary>
        /// <param name="file">上传的文件实例。</param>
        /// <param name="mediaFileName">媒体文件名称。</param>
        /// <param name="mediaType">媒体类型。</param>
        /// <param name="targetId">所属实例的Id。</param>
        /// <param name="directoryId">所属目录的Id。</param>
        /// <returns>返回文件访问的URL地址。</returns>
        Task<string> UploadAsync(IFormFile file, string mediaFileName, string mediaType = null, int targetId = 0, int directoryId = 0);
    }
}