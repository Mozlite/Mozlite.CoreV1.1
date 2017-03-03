using System.Text;

namespace Mozlite.Mvc.Routing
{
    /// <summary>
    /// 路由实例。
    /// </summary>
    public class ControllerRoute
    {
        /// <summary>
        /// 路由类型。
        /// </summary>
        public RouteType Type { get; set; }

        /// <summary>
        /// 控制器名称。
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 区域名称。
        /// </summary>
        public string Area { get; set; }
        
        /// <summary>
        /// 控制器路由名称。
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 转换为路由路径。
        /// </summary>
        /// <param name="backend">后台前缀。</param>
        /// <param name="usercenter">用户中心前缀。</param>
        /// <returns>返回路由路径。</returns>
        public string ToRoute(string backend, string usercenter)
        {
            var builder = new StringBuilder();
            if (Type == RouteType.Backend)
                builder.AppendFormat("{0}/", backend ?? "backend");
            else
                builder.AppendFormat("{0}/", usercenter ?? "self");
            if (Area != null)
                builder.AppendFormat("{0}/", Area);
            if (!string.IsNullOrEmpty(RouteName))
                builder.Append(RouteName).Append("/");
            builder.Append("{action?}/{id?}");
            return builder.ToString();
        }
    }
}