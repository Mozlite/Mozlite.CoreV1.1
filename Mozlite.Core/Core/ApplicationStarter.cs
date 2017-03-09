using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Mozlite.Core.Tasks;
using Mozlite.Data.Migrations;
using Mozlite.Mvc;
using Mozlite.Mvc.Routing;

namespace Mozlite.Core
{
    /// <summary>
    /// 应用程序起始类。
    /// </summary>
    public class ApplicationStarter
    {
        #region initializer
        private readonly IConfiguration _configuration;
        private readonly IEnumerable<RuntimeLibrary> _libraries;
        private static readonly LibraryEqualityComparer _libraryEqualityComparer = new LibraryEqualityComparer();

        /// <summary>
        /// 初始化类<see cref="ApplicationStarter"/>。
        /// </summary>
        /// <param name="configuration">配置实例。</param>
        public ApplicationStarter(IConfiguration configuration)
        {
            _configuration = configuration;
            _libraries = GetLibraries();
        }

        private IEnumerable<RuntimeLibrary> GetLibraries()
        {
            //加载Mozlite核心框架，和以网站项目开头的程序集，其他命名的程序集不加载
            var current = Assembly.GetEntryAssembly().GetName().Name.Split('.')[0];
            var libraries = DependencyContext.Default.RuntimeLibraries
                .Where(x => x.Name.StartsWith("mozlite", StringComparison.OrdinalIgnoreCase) ||
                            x.Name.StartsWith(current, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return libraries;
        }

        private class LibraryEqualityComparer : IEqualityComparer<RuntimeLibrary>
        {
            /// <summary>
            /// 确定指定的对象是否相等。
            /// </summary>
            /// <returns>
            /// 如果指定的对象相等，则为 true；否则为 false。
            /// </returns>
            /// <param name="x">要比较的第一个类型为 <see cref="RuntimeLibrary"/> 的对象。</param><param name="y">要比较的第二个类型为<see cref="RuntimeLibrary"/>的对象。</param>
            public bool Equals(RuntimeLibrary x, RuntimeLibrary y)
            {
                return x.Name.Equals(y.Name);
            }

            /// <summary>
            /// 返回指定对象的哈希代码。
            /// </summary>
            /// <returns>
            /// 指定对象的哈希代码。
            /// </returns>
            /// <param name="obj"><see cref="T:System.Object"/>，将为其返回哈希代码。</param><exception cref="T:System.ArgumentNullException"><paramref name="obj"/> 的类型为引用类型，<paramref name="obj"/> 为 null。</exception>
            public int GetHashCode(RuntimeLibrary obj)
            {
                return obj.Name.GetHashCode();
            }
        }
        #endregion

        #region configure services
        /// <summary>
        /// 配置服务。
        /// </summary>
        /// <param name="services">服务集合。</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //添加配置类
            services.AddSingleton(service => new Configuration(_configuration));
            ConfigureAssembliesServices(services);
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddControllerRoutes()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Clear();
                    options.AreaViewLocationFormats.Clear();
                    //网站试图路径：custom目录为后台修改网页代码后另存为的文件，有助于恢复原有文件
                    options.ViewLocationFormats.Add("/custom/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                    options.ViewLocationFormats.Add("/custom/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                    options.ViewLocationFormats.Add("/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                    options.ViewLocationFormats.Add("/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                    //区域试图路径：Extensions下面的路径，可以将每个区域改为扩展，这样会降低程序集的耦合度
                    var assemblyName = Assembly.GetEntryAssembly().GetName().Name;//网站的程序集名称，约定扩展程序集名称必须为“网站程序集名称.Extensions.当前扩展区域名称”
                    options.AreaViewLocationFormats.Add("/custom/Extensions/" + assemblyName + ".Extensions.{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                    options.AreaViewLocationFormats.Add("/custom/Extensions/" + assemblyName + ".Extensions.{2}/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                    options.AreaViewLocationFormats.Add("/custom/Extensions/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                    options.AreaViewLocationFormats.Add("/custom/Extensions/{2}/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                    options.AreaViewLocationFormats.Add("/custom/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                    options.AreaViewLocationFormats.Add("/Extensions/" + assemblyName + ".Extensions.{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                    options.AreaViewLocationFormats.Add("/Extensions/" + assemblyName + ".Extensions.{2}/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                    options.AreaViewLocationFormats.Add("/Extensions/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                    options.AreaViewLocationFormats.Add("/Extensions/{2}/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                    options.AreaViewLocationFormats.Add("/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
        }

        private void ConfigureAssembliesServices(IServiceCollection services)
        {
            var exportedTypes = GetExportedTypes();
            foreach (var source in exportedTypes)
            {
                if (typeof(IServiceConfigurer).IsAssignableFrom(source))
                {
                    var service = Activator.CreateInstance(source) as IServiceConfigurer;
                    service?.ConfigureServices(services);
                }
                else//注册类型
                {
                    foreach (var interfaceType in source.GetInterfaces()
                        .Where(itf => typeof(IService).IsAssignableFrom(itf)))
                    {
                        if (typeof(ISingletonService).IsAssignableFrom(interfaceType))
                        {
                            services.TryAddSingleton(interfaceType, source);
                        }
                        else if (typeof(IScopedService).IsAssignableFrom(interfaceType))
                        {
                            services.TryAddScoped(interfaceType, source);
                        }
                        else if (typeof(ISingletonServices).IsAssignableFrom(interfaceType))
                        {
                            services.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType, source));
                        }
                        else if (typeof(IScopedServices).IsAssignableFrom(interfaceType))
                        {
                            services.TryAddEnumerable(ServiceDescriptor.Scoped(interfaceType, source));
                        }
                        else if (typeof(IServices).IsAssignableFrom(interfaceType))
                        {
                            services.TryAddEnumerable(ServiceDescriptor.Transient(interfaceType, source));
                        }
                        else
                        {
                            services.TryAddTransient(interfaceType, source);
                        }
                    }
                }
            }
        }

        private IEnumerable<Type> GetExportedTypes()
        {
            var types = GetServices().ToList();
            var susppendServices = types.Select(type => type.GetTypeInfo())
                .Where(type => type.IsDefined(typeof(SuppressAttribute)))
                .ToList();
            var susppendTypes = new List<string>();
            foreach (var susppendService in susppendServices)
            {
                var suppendAttribute = susppendService.GetCustomAttribute<SuppressAttribute>();
                susppendTypes.Add(suppendAttribute.FullName);
            }
            susppendTypes = susppendTypes.Distinct().ToList();
            return types.Where(type => !susppendTypes.Contains(type.FullName))
                .ToList();
        }

        private IEnumerable<Type> GetServices()
        {
            var types = _libraries.Select(lib => Assembly.Load(new AssemblyName(lib.Name)))
                .SelectMany(assembly => assembly.GetTypes())
                .ToList();
            foreach (var type in types)
            {
                var info = type.GetTypeInfo();
                if (info.IsPublic && info.IsClass && !info.IsAbstract && typeof(IService).IsAssignableFrom(type))
                    yield return type;
            }
        }
        #endregion

        #region configure application
        /// <summary>
        /// 配置程序集应用程序。
        /// </summary>
        /// <param name="app">应用程序构建实例接口。</param>
        public void Configure(IApplicationBuilder app)
        {
            //配置程序集
            var services = app.ApplicationServices.GetService<IEnumerable<IApplicationConfigurer>>();
            foreach (var service in services)
            {
                service.Configure(app, _configuration);
            }
            //配置本地化资源
            var resources = _configuration.GetSection("Resources");
            var supportedCultures = resources
                .GetSection("Supports")
                .GetChildren()
                .Select(s => new CultureInfo(s.Key))
                .ToList();
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(resources["Default"] ?? "zh-CN"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            //数据库迁移
            app.UseMigrations();
            //配置MVC
            app.UseMvcDefault(_configuration);

            //最后执行启动项
            app.UseTasks();
        }
        #endregion
    }
}