using Mozlite.Data.Metadata;

namespace Mozlite.Extensions.Identity
{
    /// <summary>
    /// 用户组。
    /// </summary>
    [Table("Roles")]
    public class IdentityRole
    {
        /// <summary>
        /// 用户组ID。
        /// </summary>
        [Identity]
        public int RoleId { get; set; }

        /// <summary>
        /// 用户组名称。
        /// </summary>
        [Size(64)]
        public string RoleName { get; set; }

        /// <summary>
        /// 用户组名称。
        /// </summary>
        [Size(64)]
        public string NormalizedRoleName { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [Size(64)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 优先级。
        /// </summary>
        public int Priority { get; set; }
    }
}