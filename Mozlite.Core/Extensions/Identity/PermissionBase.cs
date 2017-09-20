using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mozlite.Core;
using Mozlite.Data;
using Mozlite.Data.Metadata;
using Mozlite.Mvc;

namespace Mozlite.Extensions.Identity
{
    /// <summary>
    /// 权限验证特性。
    /// </summary>
    public class PermissionAuthorizeAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// 初始化类<see cref="PermissionAuthorizeAttribute"/>。
        /// </summary>
        /// <param name="permission">当前权限名称。</param>
        public PermissionAuthorizeAttribute(string permission) : base(typeof(PermissionAuthorizeAttributeImpl))
        {
            Arguments = new object[] { new OperationAuthorizationRequirement { Name = permission } };
        }

        /// <summary>
        /// 初始化类<see cref="PermissionAuthorizeAttribute"/>。
        /// </summary>
        public PermissionAuthorizeAttribute() : base(typeof(PermissionAuthorizeAttributeImpl))
        {
        }

        private class PermissionAuthorizeAttributeImpl : Attribute, IAsyncAuthorizationFilter
        {
            private readonly ILogger _logger;
            private readonly IAuthorizationService _authorizationService;
            private readonly OperationAuthorizationRequirement _requirement;

            public PermissionAuthorizeAttributeImpl(ILogger<PermissionAuthorizeAttribute> logger, IAuthorizationService authorizationService, OperationAuthorizationRequirement requirement)
            {
                _logger = logger;
                _authorizationService = authorizationService;
                _requirement = requirement;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var result = await _authorizationService.AuthorizeAsync(context.HttpContext.User,
                    context.ActionDescriptor, _requirement);
                if (!result.Succeeded)
                    context.Result = new ChallengeResult();
            }
        }
    }

    /// <summary>
    /// 注册验证处理类。
    /// </summary>
    public class PermissionAuthorizationConfigurer : IServiceConfigurer
    {
        /// <summary>
        /// 配置服务方法。
        /// </summary>
        /// <param name="services">服务集合实例。</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }
    }

    /// <summary>
    /// 权限验证处理方法类。
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        private readonly IPermissionManager _permissionManager;
        /// <summary>
        /// 初始化类<see cref="PermissionAuthorizationHandler"/>。
        /// </summary>
        /// <param name="permissionManager">权限管理接口实例对象。</param>
        public PermissionAuthorizationHandler(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// 验证当前权限的合法性。
        /// </summary>
        /// <param name="context">验证上下文。</param>
        /// <param name="requirement">权限实例。</param>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            var permissioName = requirement?.Name;
            if (permissioName == null && context.Resource is ControllerActionDescriptor resource)
            {
                if (resource.RouteValues.TryGetValue("area", out string area))
                    permissioName = area + ".";
                permissioName += $"{resource.ControllerName}.{resource.ActionName}";
            }
            if (permissioName == null)
            {
                context.Fail();
                return;
            }
            var id = context.User.GetUserId();
            if (id > 0)
            {
                var permission = await _permissionManager.GetPermissionAsync(id, permissioName);
                if (permission == PermissionValue.Allow)
                    context.Succeed(requirement);
            }
            context.Fail();
        }
    }

    /// <summary>
    /// 权限管理接口。
    /// </summary>
    public interface IPermissionManager : ISingletonService
    {
        /// <summary>
        /// 获取当前用户的权限。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="permissioName">权限名称。</param>
        /// <returns>返回权限结果。</returns>
        Task<PermissionValue> GetPermissionAsync(int userId, string permissioName);
    }

    /// <summary>
    /// 权限管理实现类。
    /// </summary>
    public abstract class PermissionManager<TUserRole> : IPermissionManager
        where TUserRole : IdentityUserRole
    {
        private readonly IRepository<Permission> _repository;
        private readonly IRepository<PermissionInRole> _pirs;
        private readonly IModel _model;

        /// <summary>
        /// 初始化类<see cref="PermissionManager"/>。
        /// </summary>
        /// <param name="repository">数据库操作接口实例。</param>
        /// <param name="pirs">数据库操作接口。</param>
        /// <param name="model">模型接口。</param>
        protected PermissionManager(IRepository<Permission> repository, IRepository<PermissionInRole> pirs, IModel model)
        {
            _repository = repository;
            _pirs = pirs;
            _model = model;
        }

        /// <summary>
        /// 获取当前用户的权限。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="permissioName">权限名称。</param>
        /// <returns>返回权限结果。</returns>
        public async Task<PermissionValue> GetPermissionAsync(int userId, string permissioName)
        {
            var values = await _pirs.AsQueryable()
                .InnerJoin<TUserRole>((p, r) => p.RoleId == r.RoleId)
                .InnerJoin<Permission>((pi, p) => pi.PermissionId == p.Id)
                .Where<TUserRole>(ur => ur.UserId == userId)
                .Where<Permission>(p => p.Name == permissioName)
                .Select(x => x.Value)
                .AsEnumerableAsync(r => (PermissionValue)r.GetInt32(0));
            if (values.Any(x => x == PermissionValue.Deny))
                return PermissionValue.Deny;
            if (values.Any(x => x == PermissionValue.Allow))
                return PermissionValue.Allow;
            return PermissionValue.NotSet;
        }
    }

    public class PermissionManagerImpl : PermissionManager<IdentityUserRole>
    {
        /// <summary>
        /// 初始化类<see cref="PermissionManager{TUserRole}"/>。
        /// </summary>
        /// <param name="repository">数据库操作接口实例。</param>
        /// <param name="pirs">数据库操作接口。</param>
        /// <param name="model">模型接口。</param>
        public PermissionManagerImpl(IRepository<Permission> repository, IRepository<PermissionInRole> pirs, IModel model) : base(repository, pirs, model)
        {
        }
    }

    /// <summary>
    /// 权限实体。
    /// </summary>
    [Table("core_Permissions")]
    public class Permission
    {
        /// <summary>
        /// 唯一Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        [Size(64)]
        public string Name { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [Size(256)]
        public string Description { get; set; }
    }

    /// <summary>
    /// 角色权限。
    /// </summary>
    [Table("core_Permissions_In_Roles")]
    public class PermissionInRole
    {
        /// <summary>
        /// 角色Id。
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 权限Id。
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// 当前权限设定。
        /// </summary>
        public PermissionValue Value { get; set; }
    }

    /// <summary>
    /// 权限的值。
    /// </summary>
    public enum PermissionValue
    {
        /// <summary>
        /// 禁止。
        /// </summary>
        Deny,
        /// <summary>
        /// 未设置。
        /// </summary>
        NotSet,
        /// <summary>
        /// 允许。
        /// </summary>
        Allow,
    }
}