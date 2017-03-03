namespace Mozlite.Extensions
{
    /// <summary>
    /// 对象状态。
    /// </summary>
    public enum ObjectStatus
    {
        /// <summary>
        /// 禁用。
        /// </summary>
        Disabled = -999,

        /// <summary>
        /// 验证失败。
        /// </summary>
        Disapproved = -2,

        /// <summary>
        /// 等待验证。
        /// </summary>
        PaddingApproved = -1,

        /// <summary>
        /// 正常。
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 关闭，问答。
        /// </summary>
        Closed = 1,

        /// <summary>
        /// 解决，完成，问答。
        /// </summary>
        Completed = 2,
    }
}