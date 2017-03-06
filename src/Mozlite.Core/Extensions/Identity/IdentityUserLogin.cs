using Mozlite.Data.Metadata;

namespace Mozlite.Extensions.Identity
{
    /// <summary>
    /// 用户登陆实例。
    /// </summary>
    [Table("Users_Logins")]
    public class IdentityUserLogin
    {
        /// <summary>
        /// 登陆提供者(如：facebook, google)。
        /// </summary>
        [Key]
        [Size(64)]
        public string LoginProvider { get; set; }

        /// <summary>
        /// 获取登录提供者提供的唯一Id。
        /// </summary>
        [Key]
        [Size(256)]
        public string ProviderKey { get; set; }

        /// <summary>
        /// 登陆提供者友好名称。
        /// </summary>
        [Size(64)]
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// 用户登录的用户ID。
        /// </summary>
        public int UserId { get; set; }
    }
}