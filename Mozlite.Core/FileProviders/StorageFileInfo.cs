using Mozlite.Data.Metadata;

namespace Mozlite.FileProviders
{
    /// <summary>
    /// 存储文件。
    /// </summary>
    [Table("core_Medias_Storages")]
    public class StorageFileInfo
    {
        /// <summary>
        /// 文件Id。
        /// </summary>
        [Identity]
        public int FileId { get; set; }
        
        /// <summary>
        /// 文件名。
        /// </summary>
        [Size(32)]
        public string Name { get; set; }

        /// <summary>
        /// 内容类型。
        /// </summary>
        [Size(256)]
        public string ContentType { get; set; }

        /// <summary>
        /// 引用次数。
        /// </summary>
        public int Reference { get; set; }

        /// <summary>
        /// 大小。
        /// </summary>
        public long Length { get; set; }

        private string _path;
        /// <summary>
        /// 媒体路径。
        /// </summary>
        public string Path
        {
            get
            {
                if (_path == null && Name != null)
                    _path = $"{Name[1]}\\{Name[3]}\\{Name[12]}\\{Name[16]}\\{Name[20]}\\{Name}.moz";
                return _path;
            }
        }
    }
}