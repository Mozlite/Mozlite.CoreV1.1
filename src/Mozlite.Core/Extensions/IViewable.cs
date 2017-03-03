namespace Mozlite.Extensions
{
    /// <summary>
    /// 访问统计接口。
    /// </summary>
    public interface IViewable
    {
        /// <summary>
        /// 唯一Id。
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 总访问量。
        /// </summary>
        int Views { get; set; }

        /// <summary>
        /// 今日访问量。
        /// </summary>
        int TodayViews { get; set; }

        /// <summary>
        /// 星期访问量。
        /// </summary>
        int WeekViews { get; set; }

        /// <summary>
        /// 本月访问量。
        /// </summary>
        int MonthViews { get; set; }
    }
}