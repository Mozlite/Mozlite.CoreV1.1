namespace Mozlite.FileProviders
{
    /// <summary>
    /// 文件存储选项。
    /// </summary>
    public class FileProviderOptions
    {
        /// <summary>
        /// 媒体文件存储路径。
        /// </summary>
        public string MediaPath { get; set; } = "_media";

        /// <summary>
        /// 临时文件夹路径。
        /// </summary>
        public string TempPath { get; set; } = "_temp";
    }
}