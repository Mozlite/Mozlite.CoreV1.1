using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mozlite.Core;
using Mozlite.Data;
using Mozlite.Data.Metadata;
using Mozlite.Properties;
using Newtonsoft.Json;

namespace Mozlite.Extensions.Identity
{
    /// <summary>
    /// 权限基类。
    /// </summary>
    public abstract class PermissionBase
    {
        /// <summary>
        /// 用户组Id。
        /// </summary>
        [JsonIgnore]
        public int RoleId { get; set; }

        /// <summary>
        /// 扩展名称。
        /// </summary>
        [JsonIgnore]
        public string ExtensionName { get; set; }
    }

    /// <summary>
    /// 权限表格。
    /// </summary>
    [Table("core_Permissions")]
    public class PermissionAdapter
    {
        /// <summary>
        /// 用户组Id。
        /// </summary>
        [Key]
        public int RoleId { get; set; }

        /// <summary>
        /// 扩展名称。
        /// </summary>
        [Key]
        [Size(32)]
        public string ExtensionName { get; set; }

        /// <summary>
        /// 权限JSON。
        /// </summary>
        public string Permissions { get; set; }

        internal PermissionAdapter(PermissionBase permission)
        {
            RoleId = permission.RoleId;
            ExtensionName = permission.ExtensionName;
            Permissions = JsonConvert.SerializeObject(permission);
        }

        internal TPermission ToPermission<TPermission>() where TPermission : PermissionBase, new()
        {
            var permission = JsonConvert.DeserializeObject<TPermission>(Permissions);
            permission.ExtensionName = ExtensionName;
            permission.RoleId = RoleId;
            return permission;
        }
    }

    /// <summary>
    /// 验证接口。
    /// </summary>
    /// <typeparam name="TPermission">权限类型。</typeparam>
    public interface IAuthorizer<out TPermission>
        where TPermission : PermissionBase, new()
    {
        /// <summary>
        /// 验证当前权限是否合法。
        /// </summary>
        /// <param name="func">验证方法。</param>
        /// <returns>返回验证结果。</returns>
        bool IsValid(Func<TPermission, bool> func);

        /// <summary>
        /// 验证当前权限是否合法。
        /// </summary>
        /// <param name="func">获取当前字符串属性值。</param>
        /// <param name="permission">包含的字符。</param>
        /// <returns>返回验证结果。</returns>
        bool IsValid(Func<TPermission, string> func, string permission);
    }

    /// <summary>
    /// 权限管理接口。
    /// </summary>
    public interface IPermissionManager : ISingletonService
    {
        /// <summary>
        /// 保存权限。
        /// </summary>
        /// <param name="permission">权限实例对象。</param>
        /// <returns>返回保存结果。</returns>
        DataResult Save(PermissionBase permission);

        /// <summary>
        /// 保存权限。
        /// </summary>
        /// <param name="permission">权限实例对象。</param>
        /// <returns>返回保存结果。</returns>
        Task<DataResult> SaveAsync(PermissionBase permission);

        /// <summary>
        /// 拼接权限。
        /// </summary>
        /// <typeparam name="TPermission">权限类型。</typeparam>
        /// <param name="permission">当前权限。</param>
        /// <param name="permissions">权限列表。</param>
        /// <returns>返回权限实例对象。</returns>
        TPermission MergePermission<TPermission>(TPermission permission, params TPermission[] permissions)
            where TPermission : PermissionBase, new();

        /// <summary>
        /// 加载当前扩展模块的权限列表。
        /// </summary>
        /// <typeparam name="TPermission">权限类型。</typeparam>
        /// <param name="extensionName">扩展模块名称。</param>
        /// <returns>返回权限列表。</returns>
        IEnumerable<TPermission> LoadPermissions<TPermission>(string extensionName)
            where TPermission : PermissionBase, new();
    }

    /// <summary>
    /// 权限管理实现类。
    /// </summary>
    public class PermissionManager : IPermissionManager
    {
        private readonly IRepository<PermissionAdapter> _repository;
        private readonly IModel _model;

        /// <summary>
        /// 初始化类<see cref="PermissionManager"/>。
        /// </summary>
        /// <param name="repository">数据库操作接口实例。</param>
        /// <param name="model">模型接口。</param>
        public PermissionManager(IRepository<PermissionAdapter> repository, IModel model)
        {
            _repository = repository;
            _model = model;
        }

        /// <summary>
        /// 保存权限。
        /// </summary>
        /// <param name="permission">权限实例对象。</param>
        /// <returns>返回保存结果。</returns>
        public DataResult Save(PermissionBase permission)
        {
            var adapter = new PermissionAdapter(permission);
            if (_repository.Any(x => x.RoleId == permission.RoleId && x.ExtensionName == permission.ExtensionName))
                return DataResult.FromResult(_repository.Update(x => x.RoleId == permission.RoleId && x.ExtensionName == permission.ExtensionName, new { adapter.Permissions }), DataAction.Updated);
            return DataResult.FromResult(_repository.Create(adapter), DataAction.Created);
        }

        /// <summary>
        /// 保存权限。
        /// </summary>
        /// <param name="permission">权限实例对象。</param>
        /// <returns>返回保存结果。</returns>
        public async Task<DataResult> SaveAsync(PermissionBase permission)
        {
            var adapter = new PermissionAdapter(permission);
            if (await _repository.AnyAsync(x => x.RoleId == permission.RoleId && x.ExtensionName == permission.ExtensionName))
                return DataResult.FromResult(await _repository.UpdateAsync(x => x.RoleId == permission.RoleId && x.ExtensionName == permission.ExtensionName, new { adapter.Permissions }), DataAction.Updated);
            return DataResult.FromResult(await _repository.CreateAsync(adapter), DataAction.Created);
        }

        /// <summary>
        /// 拼接权限。
        /// </summary>
        /// <typeparam name="TPermission">权限类型。</typeparam>
        /// <param name="permission">当前权限。</param>
        /// <param name="permissions">权限列表。</param>
        /// <returns>返回权限实例对象。</returns>
        public TPermission MergePermission<TPermission>(TPermission permission, params TPermission[] permissions) where TPermission : PermissionBase, new()
        {
            var properties = _model.GetEntity(typeof(TPermission)).GetProperties().ToList();
            foreach (var current in permissions)
            {
                foreach (var property in properties)
                {
                    var merger = property.Get(current);
                    if (merger == null)
                        continue;
                    var basic = property.Get(permission);
                    switch (basic)
                    {
                        case Boolean bPermission when bPermission:
                            property.Set(permission, merger);
                            break;
                        case int iPermission:
                            property.Set(permission, Math.Max(iPermission, (int)merger));
                            break;
                        case long lPermission:
                            property.Set(permission, Math.Max(lPermission, (long)merger));
                            break;
                        case string sBasic:
                            var sBasics = sBasic.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Concat(merger.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                .Select(x => x.Trim())
                                .Distinct(StringComparer.OrdinalIgnoreCase);
                            property.Set(permission, string.Join(",", sBasics));
                            break;
                        case null:
                            property.Set(permission, merger);
                            break;
                        default:
                            throw new Exception(string.Format(Resources.NotSupportedPropertyType, property.ClrType));
                    }
                }
            }
            return permission;
        }

        /// <summary>
        /// 加载当前扩展模块的权限列表。
        /// </summary>
        /// <typeparam name="TPermission">权限类型。</typeparam>
        /// <param name="extensionName">扩展模块名称。</param>
        /// <returns>返回权限列表。</returns>
        public IEnumerable<TPermission> LoadPermissions<TPermission>(string extensionName)
            where TPermission : PermissionBase, new()
        {
            return _repository.Load(x => x.ExtensionName == extensionName)
                .Select(x => x.ToPermission<TPermission>());
        }
    }
}