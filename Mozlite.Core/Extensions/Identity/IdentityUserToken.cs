using Mozlite.Data.Metadata;

namespace Mozlite.Extensions.Identity
{
    /// <summary>
    /// 用户登陆提供者的一些信息存储。
    /// </summary>
    [Table("core_Users_Tokens")]
    public class IdentityUserToken
    {
        /// <summary>
        /// 用户ID。
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// 登陆提供者。
        /// </summary>
        [Key]
        [Size(256)]
        public string LoginProvider { get; set; }

        /// <summary>
        /// 标识唯一键。
        /// </summary>
        [Key]
        [Size(256)]
        public string Name { get; set; }

        /// <summary>
        /// 当前标识的值。
        /// </summary>
        public string Value { get; set; }
    }
}