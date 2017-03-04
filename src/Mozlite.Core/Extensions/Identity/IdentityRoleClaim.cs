using Mozlite.Data.Metadata;

namespace Mozlite.Extensions.Identity
{
    /// <summary>
    /// 用户组声明类。
    /// </summary>
    [Table("Roles_Claims")]
    public class IdentityRoleClaim
    {
        /// <summary>
        /// 标识ID。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 用户组ID。
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 声明类型。
        /// </summary>
        [Size(256)]
        public virtual string ClaimType { get; set; }

        /// <summary>
        /// 声明值。
        /// </summary>
        public virtual string ClaimValue { get; set; }
    }
}