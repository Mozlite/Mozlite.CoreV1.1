using System;
using Mozlite.Data.Metadata;

namespace Mozlite.FileProviders
{
    /// <summary>
    /// 媒体文件。
    /// </summary>
    [Table("Medias_Files")]
    public class MediaFileInfo
    {
        /// <summary>
        /// 媒体文件Id。
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 存储文件Id。
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// 文件名称。
        /// </summary>
        public string FileName => Id.ToString("N") + Extension;

        /// <summary>
        /// 标题，描述信息。
        /// </summary>
        [Size(256)]
        public string Title { get; set; }

        /// <summary>
        /// 扩展名。
        /// </summary>
        [Size(32)]
        public string Extension { get; set; }

        private bool? _isImage;
        /// <summary>
        /// 是否为图片文件。
        /// </summary>
        public bool IsImage
        {
            get
            {
                if (_isImage == null)
                    _isImage = ContentType.GetContentType(Extension).StartsWith("image/");
                return _isImage.Value;
            }
        }

        /// <summary>
        /// 媒体类型。
        /// </summary>
        [Size(64)]
        public string Type { get; set; }

        /// <summary>
        /// 所属类型的Id。
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// 文件目录Id。
        /// </summary>
        public int DirectoryId { get; set; }

        /// <summary>
        /// 新建时间。
        /// </summary>
        [Ignore(Ignore.Update)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 文件大小。
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// 访问地址。
        /// </summary>
        public string Url => $"/media/{Type}/{FileName}".ToLower();
    }
}