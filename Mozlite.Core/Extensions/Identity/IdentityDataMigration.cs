using Mozlite.Data.Migrations;
using Mozlite.Data.Migrations.Builders;
using Mozlite.Properties;

namespace Mozlite.Extensions.Identity
{
    /// <summary>
    /// Identity数据库迁移。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TRole">用户组类型。</typeparam>
    /// <typeparam name="TUserClaim">用户声明类型。</typeparam>
    /// <typeparam name="TUserLogin">用户登陆类型。</typeparam>
    /// <typeparam name="TUserRole">用户所在组类型。</typeparam>
    /// <typeparam name="TRoleClaim">用户组声明类型。</typeparam>
    /// <typeparam name="TUserToken">用户标识类型。</typeparam>
    public abstract class IdentityDataMigration<TUser, TRole, TUserClaim, TRoleClaim, TUserLogin, TUserRole, TUserToken> : DataMigration<TUser>
        where TUser : IdentityUser, new()
        where TRole : IdentityRole, new()
        where TUserClaim : IdentityUserClaim, new()
        where TRoleClaim : IdentityRoleClaim, new()
        where TUserRole : IdentityUserRole, new()
        where TUserLogin : IdentityUserLogin, new()
        where TUserToken : IdentityUserToken, new()
    {
        /// <summary>
        /// 创建操作。
        /// </summary>
        /// <param name="builder">迁移构建实例对象。</param>
        protected override void Create(MigrationBuilder<TUser> builder)
        {
            builder.CreateTable(table =>
            {
                table
                    .Column(x => x.UserId)
                    .Column(x => x.UserName, nullable: false)
                    .Column(x => x.NickName)
                    .Column(x => x.NormalizedUserName, nullable: false)
                    .Column(x => x.PasswordHash, nullable: false)
                    .Column(x => x.Email)
                    .Column(x => x.NormalizedEmail)
                    .Column(x => x.EmailConfirmed)
                    .Column(x => x.SecurityStamp)
                    .Column(x => x.PhoneNumber)
                    .Column(x => x.PhoneNumberConfirmed)
                    .Column(x => x.TwoFactorEnabled)
                    .Column(x => x.LockoutEnd)
                    .Column(x => x.LockoutEnabled)
                    .Column(x => x.AccessFailedCount)
                    .Column(x => x.CreatedIP)
                    .Column(x => x.CreatedDate)
                    .Column(x => x.LoginIP)
                    .Column(x => x.LastLoginDate)
                    .Column(x => x.UpdatedDate)
                    .Column(x => x.Avatar);
                Create(table);
            });
            builder.CreateIndex(x => x.NormalizedUserName, true);
            builder.CreateIndex(x => x.NormalizedEmail);

            builder.CreateTable<TRole>(table => table
                .Column(x => x.RoleId)
                .Column(x => x.RoleName, nullable: false)
                .Column(x => x.NormalizedRoleName, nullable: false)
                .Column(x => x.DisplayName)
                .Column(x => x.Priority));
            builder.CreateIndex<TRole>(x => x.NormalizedRoleName, true);

            builder.CreateTable<TUserRole>(table => table
                .Column(x => x.UserId)
                .Column(x => x.RoleId)
                .ForeignKey<TUser>(x => x.UserId, onDelete: ReferentialAction.Cascade)
                .ForeignKey<TRole>(x => x.RoleId, onDelete: ReferentialAction.Cascade));

            builder.CreateTable<TRoleClaim>(table => table
                .Column(x => x.Id)
                .Column(x => x.ClaimType, nullable: false)
                .Column(x => x.ClaimValue)
                .Column(x => x.RoleId)
                .ForeignKey<TRole>(x => x.RoleId, onDelete: ReferentialAction.Cascade));

            builder.CreateTable<TUserClaim>(table => table
                .Column(x => x.Id)
                .Column(x => x.ClaimType, nullable: false)
                .Column(x => x.ClaimValue)
                .Column(x => x.UserId)
                .ForeignKey<TUser>(x => x.UserId, onDelete: ReferentialAction.Cascade));

            builder.CreateTable<TUserLogin>(table => table
                .Column(x => x.LoginProvider, nullable: false)
                .Column(x => x.ProviderKey, nullable: false)
                .Column(x => x.ProviderDisplayName)
                .Column(x => x.UserId)
                .ForeignKey<TUser>(x => x.UserId, onDelete: ReferentialAction.Cascade));

            builder.CreateTable<TUserToken>(table => table
                .Column(x => x.LoginProvider, nullable: false)
                .Column(x => x.Name, nullable: false)
                .Column(x => x.Value)
                .Column(x => x.UserId)
                .ForeignKey<TUser>(x => x.UserId, onDelete: ReferentialAction.Cascade));
        }

        /// <summary>
        /// 添加用户定义列。
        /// </summary>
        /// <param name="builder">用户表格定义实例。</param>
        protected virtual void Create(CreateTableBuilder<TUser> builder) { }

        public virtual void Up1(MigrationBuilder builder)
        {
            builder.SqlCreate(new TRole
            {
                RoleName = IdentitySettings.Administrator,
                NormalizedRoleName = IdentitySettings.Administrator.ToUpper(),
                Priority = int.MaxValue,
                DisplayName = Resources.Administrator
            });
            builder.SqlCreate(new TRole
            {
                RoleName = IdentitySettings.Register,
                NormalizedRoleName = IdentitySettings.Register.ToUpper(),
                DisplayName = Resources.Register
            });
        }

        public virtual void Down1(MigrationBuilder builder)
        {
            builder.SqlDelete<TRole>(role => role.RoleId > 0);
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Destroy(MigrationBuilder builder)
        {
            builder.DropTable<TUserRole>();
            builder.DropTable<TRoleClaim>();
            builder.DropTable<TRole>();
            builder.DropTable<TUserToken>();
            builder.DropTable<TUserLogin>();
            builder.DropTable<TUserClaim>();
            base.Destroy(builder);
        }
    }
}