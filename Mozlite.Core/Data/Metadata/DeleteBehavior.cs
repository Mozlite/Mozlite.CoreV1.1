namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 外键删除操作执行的行为。
    /// </summary>
    public enum DeleteBehavior
    {
        /// <summary>
        /// 不进行任何操作。
        /// </summary>
        Restrict,

        /// <summary>
        /// 当主键删除时将外键设置为null。
        /// </summary>
        SetNull,

        /// <summary>
        /// 级联删除。
        /// </summary>
        Cascade
    }
}