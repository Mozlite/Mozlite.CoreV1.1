using Mozlite.Data.Metadata;

namespace Mozlite.Extensions.Identity
{
    /// <summary>
    /// 用户和用户组。
    /// </summary>
    [Table("Users_Roles")]
    public class IdentityUserRole
    {
        /// <summary>
        /// 用户组ID。
        /// </summary>
        [Key]
        public int RoleId { get; set; }

        /// <summary>
        /// 用户ID。
        /// </summary>
        [Key]
        public int UserId { get; set; }
    }
}