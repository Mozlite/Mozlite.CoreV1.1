using System.IO;

namespace Mozlite.FileProviders
{
    /// <summary>
    /// 存储文件。
    /// </summary>
    public class StorageFile
    {
        internal StorageFile(string physicalPath, string contentType)
        {
            PhysicalPath = physicalPath;
            ContentType = contentType;
        }

        /// <summary>
        /// 物理路径。
        /// </summary>
        public string PhysicalPath { get; }

        /// <summary>
        /// 内容类型。
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        /// 新建读取文件流。
        /// </summary>
        /// <returns>返回只读文件流。</returns>
        public Stream CreateOpenReadStream()
        {
            return new FileStream(PhysicalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 65536, FileOptions.Asynchronous | FileOptions.SequentialScan);
        }
    }
}