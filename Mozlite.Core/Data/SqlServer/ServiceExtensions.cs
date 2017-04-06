using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mozlite.Data.Internal;
using Mozlite.Data.Migrations;
using Mozlite.Data.Migrations.Models;
using Mozlite.Data.Query;
using Mozlite.Data.Query.Translators;
using Mozlite.Data.SqlServer.Migrations;
using Mozlite.Data.SqlServer.Query;
using Mozlite.Data.SqlServer.Query.Translators;

namespace Mozlite.Data.SqlServer
{
    /// <summary>
    /// 数据库相关的服务器扩展。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加SQLServer数据库服务。
        /// </summary>
        /// <param name="serviceCollection">服务集合。</param>
        /// <param name="configuration">配置实例。</param>
        /// <returns>返回服务集合实例。</returns>
        public static IServiceCollection AddSqlServer(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionString = configuration["Data:Name"];
            if (connectionString == null)
                connectionString = configuration["Data:ConnectionString"];
            else
                connectionString = $"Data Source=.;Initial Catalog={connectionString};Integrated Security=True";
            return serviceCollection.AddSqlServer(options =>
            {
                options.ConnectionString = connectionString;
                options.Prefix = configuration["Data:Prefix"];
                options.Schema = configuration["Data:Schema"];
            });
        }

        /// <summary>
        /// 添加SQLServer数据库服务。
        /// </summary>
        /// <param name="serviceCollection">服务集合。</param>
        /// <param name="options">数据源选项。</param>
        /// <returns>返回服务集合实例。</returns>
        public static IServiceCollection AddSqlServer(this IServiceCollection serviceCollection, Action<DatabaseOptions> options)
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));
            Check.NotNull(options, nameof(options));
            var source = new DatabaseOptions();
            options(source);

            return serviceCollection
                .AddSingleton<IDatabase, SqlServerDatabase>()
                .Configure<DatabaseOptions>(o =>
                {
                    o.ConnectionString = source.ConnectionString;
                    o.Prefix = source.Prefix?.Trim();
                    o.Schema = source.Schema;
                    o.Provider = "SqlServer";
                })
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddTransient<IMigrator, Migrator>()
                .AddTransient<IMigrationRepository, SqlServerMigrationRepository>()
                .AddTransient<IMigrationsSqlGenerator, MigrationsSqlServerGenerator>()
                .AddSingleton<ISqlGenerator, SqlServerGenerator>()
                .AddSingleton<ITypeMapper, SqlServerTypeMapper>()
                .AddSingleton<ISqlHelper, SqlServerHelper>()
                .AddSingleton<IMemberTranslator, SqlServerCompositeMemberTranslator>()
                .AddSingleton<IMethodCallTranslator, SqlServerCompositeMethodCallTranslator>()
                .AddSingleton<IExpressionFragmentTranslator, SqlServerCompositeExpressionFragmentTranslator>()
                .AddSingleton<IExpressionVisitorFactory, SqlServerExpressionVisitorFactory>();
        }
    }
}