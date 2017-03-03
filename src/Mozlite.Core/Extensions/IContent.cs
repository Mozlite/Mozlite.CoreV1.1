namespace Mozlite.Extensions
{
    /// <summary>
    /// 内容接口。
    /// </summary>
    public interface IContent
    {
        /// <summary>
        /// 唯一Id。
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 内容。
        /// </summary>
        string Body { get; set; }

        /// <summary>
        /// 格式化HTML的内容字符串。
        /// </summary>
        string HtmlBody { get; set; }
    }
}