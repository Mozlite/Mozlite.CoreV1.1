using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mozlite.Extensions.Identity;
using Mozlite.Mvc.Routing;

namespace Mozlite.Mvc
{
    /// <summary>
    /// MVC扩展类。
    /// </summary>
    public static class MvcExtensions
    {
        /// <summary>
        /// 添加默认Mvc。
        /// </summary>
        /// <param name="app">APP构建实例对象。</param>
        /// <param name="configuration">配置实例对象。</param>
        /// <returns>APP构建实例对象。</returns>
        public static IApplicationBuilder UseMvcDefault(this IApplicationBuilder app, IConfiguration configuration)
        {
            var section = configuration.GetSection("routes");
            return app.UseMvc(builder =>
             {
                 var routes = app.ApplicationServices.GetRequiredService<ControllerRouteCollection>();
                 foreach (var route in routes)
                 {
                     builder.MapLowerCaseRoute($"{route.Area}-{route.ControllerName}", route.ToRoute(section["backend"], section["usercenter"]), new { controller = route.ControllerName, action = "index", area = route.Area });
                 }
                 builder.MapLowerCaseRoute("area-default", "{area:exists}/{controller}/{action=Index}/{id?}")
                        .MapLowerCaseRoute("default", "{controller=Home}/{action=Index}/{id?}");
             });
        }

        /// <summary>
        /// 获取用户的IP地址。
        /// </summary>
        /// <param name="httpContext">当前HTTP上下文。</param>
        /// <returns>返回当前用户IP地址。</returns>
        public static string GetUserAddress(this HttpContext httpContext)
        {
            var ipAddress = httpContext.Connection?.RemoteIpAddress?.ToString();
            if (ipAddress != null)
                return ipAddress;
            var xff = httpContext.Request.Headers["x-forwarded-for"];
            if (xff.Count > 0)
            {
                ipAddress = xff.FirstOrDefault();
                return ipAddress?.Split(':').FirstOrDefault();
            }
            return null;
        }

        private const string UserIdClaimDefine = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        /// <summary>
        /// 获取当前登陆用户的Id。
        /// </summary>
        /// <param name="claims">当前用户接口实例。</param>
        /// <returns>返回用户Id，如果未登录则返回0。</returns>
        public static int GetUserId(this ClaimsPrincipal claims)
        {
            return claims.FindFirstValue(UserIdClaimDefine).AsInt32() ?? 0;
        }

        /// <summary>
        /// 获取当前用户的用户名称。
        /// </summary>
        /// <param name="claims">当前用户接口实例。</param>
        /// <returns>返回用户名称，如果未登录则返回“Anonymous”。</returns>
        public static string GetUserName(this ClaimsPrincipal claims)
        {
            return claims.FindFirstValue(ClaimsIdentity.DefaultNameClaimType) ?? IdentitySettings.Anonymous;
        }

        /// <summary>
        /// 获取试图名称。
        /// </summary>
        /// <param name="viewContext">试图上下文。</param>
        /// <returns>返回当前视图名称。</returns>
        public static string GetActionName(this ViewContext viewContext)
        {
            return viewContext.ActionDescriptor.RouteValues["action"];
        }

        /// <summary>
        /// 获取区域名称。
        /// </summary>
        /// <param name="viewContext">试图上下文。</param>
        /// <returns>返回当前视图区域名称。</returns>
        public static string GetAreaName(this ViewContext viewContext)
        {
            return viewContext.ActionDescriptor.RouteValues["area"];
        }

        /// <summary>
        /// 获取控制器名称。
        /// </summary>
        /// <param name="viewContext">试图上下文。</param>
        /// <returns>返回当前视图控制器名称。</returns>
        public static string GetControllerName(this ViewContext viewContext)
        {
            return viewContext.ActionDescriptor.RouteValues["controller"];
        }

        /// <summary>
        /// 显示内容的HTML字符串，如果<paramref name="content"/>为空则不显示任何字符串。
        /// </summary>
        /// <param name="helper">HTML辅助接口实例对象。</param>
        /// <param name="content">当前要显示的内容。</param>
        /// <param name="format">显示格式。</param>
        /// <returns>返回要显示的HTML字符串。</returns>
        public static IHtmlContent Raw(this IHtmlHelper helper, object content, string format)
        {
            return helper.Raw(content?.ToString(), format);
        }

        /// <summary>
        /// 显示内容的HTML字符串，如果<paramref name="content"/>为空则不显示任何字符串。
        /// </summary>
        /// <param name="helper">HTML辅助接口实例对象。</param>
        /// <param name="content">当前要显示的内容。</param>
        /// <param name="format">显示格式。</param>
        /// <returns>返回要显示的HTML字符串。</returns>
        public static IHtmlContent Raw(this IHtmlHelper helper, string content, string format)
        {
            if (string.IsNullOrWhiteSpace(content))
                return null;
            if (format != null)
                content = string.Format(format, content);
            return helper.Raw(content);
        }

        /// <summary>
        /// 显示内容的HTML字符串，如果<paramref name="content"/>为空则显示<paramref name="replacement"/>字符串。
        /// </summary>
        /// <param name="helper">HTML辅助接口实例对象。</param>
        /// <param name="content">当前要显示的内容。</param>
        /// <param name="replacement">替换显示的字符串。</param>
        /// <returns>返回要显示的HTML字符串。</returns>
        public static IHtmlContent Alt(this IHtmlHelper helper, string content, string replacement)
        {
            if (string.IsNullOrWhiteSpace(content))
                content = replacement;
            return helper.Raw(content);
        }

        /// <summary>
        /// 显示内容的HTML字符串，如果<paramref name="key"/>的查询字符串为空则不显示任何字符串。
        /// </summary>
        /// <param name="helper">HTML辅助接口实例对象。</param>
        /// <param name="key">当前要显示的内容的查询字符串。</param>
        /// <param name="defaultValue">如果查询字符串为空显示的内容。</param>
        /// <returns>返回要显示的HTML字符串。</returns>
        public static IHtmlContent Query(this IHtmlHelper helper, string key, string defaultValue)
        {
            string query = helper.ViewContext.HttpContext.Request.Query[key];
            if (string.IsNullOrWhiteSpace(query))
                query = defaultValue;
            return helper.Raw(query);
        }

        /// <summary>
        /// 显示内容的HTML字符串，如果<paramref name="key"/>的查询字符串为空则不显示任何字符串。
        /// </summary>
        /// <param name="helper">HTML辅助接口实例对象。</param>
        /// <param name="key">当前要显示的内容的查询字符串。</param>
        /// <returns>返回要显示的HTML字符串。</returns>
        public static IHtmlContent Query(this IHtmlHelper helper, string key)
        {
            string query = helper.ViewContext.HttpContext.Request.Query[key];
            if (string.IsNullOrWhiteSpace(query))
                return null;
            return helper.Raw(query);
        }

        /// <summary>
        /// 获取查询字符串。
        /// </summary>
        /// <param name="request">当前HTTP请求。</param>
        /// <param name="key">唯一键。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>返回当前键的查询字符串。</returns>
        public static string GetString(this HttpRequest request, string key, string defaultValue = null)
        {
            string query = request.Query[key];
            return query ?? defaultValue;
        }

        /// <summary>
        /// 获取查询整形数值。
        /// </summary>
        /// <param name="request">当前HTTP请求。</param>
        /// <param name="key">唯一键。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>返回当前键的查询整形数值。</returns>
        public static int GetInt32(this HttpRequest request, string key, int defaultValue = 0)
        {
            return request.GetString(key).AsInt32() ?? defaultValue;
        }

        /// <summary>
        /// 获取路由中的整形值。
        /// </summary>
        /// <param name="data">路由数据。</param>
        /// <param name="key">路由键。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>返回当前值。</returns>
        public static int GetInt32(this RouteData data, string key, int defaultValue = 0)
        {
            try
            {
                return Convert.ToInt32(data.Values[key]);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取路由中的字符串值。
        /// </summary>
        /// <param name="data">路由数据。</param>
        /// <param name="key">路由键。</param>
        /// <returns>返回当前值。</returns>
        public static string GetString(this RouteData data, string key)
        {
            object value;
            data.Values.TryGetValue(key, out value);
            return value?.ToString();
        }

        /// <summary>
        /// 获取当前Action对应的地址。
        /// </summary>
        /// <param name="urlHelper">URL辅助接口。</param>
        /// <param name="action">试图。</param>
        /// <param name="controller">控制器。</param>
        /// <param name="values">路由对象。</param>
        /// <returns>返回当前Url地址。</returns>
        public static string ActionUrl(this IUrlHelper urlHelper, string action, string controller = null, object values = null)
        {
            var areaName = urlHelper.ActionContext.ActionDescriptor.RouteValues["area"];
            controller = controller ?? urlHelper.ActionContext.ActionDescriptor.RouteValues["controller"];
            if (values == null)
                return urlHelper.Action(action, controller, areaName == null ? null : new { area = areaName });
            var routes = new RouteValueDictionary(values);
            if (routes.ContainsKey("area") || areaName == null)
                return urlHelper.Action(action, controller, routes);
            routes.Add("area", areaName);
            return urlHelper.Action(action, controller, routes);
        }
    }
}