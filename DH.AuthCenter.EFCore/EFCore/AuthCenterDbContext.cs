using Abp.EntityFrameworkCore;
using DH.AuthCenter.Application.Editions;
using DH.AuthCenter.Authorization;
using DH.AuthCenter.Authorization.Roles;
using DH.AuthCenter.Authorization.Users;
using DH.AuthCenter.Configuration;
using DH.AuthCenter.Core.MultiTenancy;
using DH.AuthCenter.Organizations;
using Microsoft.EntityFrameworkCore;

namespace DH.AuthCenter.EFCore
{
    public class AuthCenterDbContext : AbpDbContext
    {
        /// <summary>
        /// Roles.
        /// </summary>
        public virtual DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Users.
        /// </summary>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// User roles.
        /// </summary>
        public virtual DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// User claims
        /// </summary>
        public virtual DbSet<UserClaim> UserClaims { get; set; }

        /// <summary>
        /// Role claims.
        /// </summary>
        public virtual DbSet<RoleClaim> RoleClaims { get; set; }

        /// <summary>
        /// Permissions.
        /// </summary>
        public virtual DbSet<PermissionSetting> Permissions { get; set; }

        /// <summary>
        /// Role permissions.
        /// </summary>
        public virtual DbSet<RolePermissionSetting> RolePermissions { get; set; }

        /// <summary>
        /// User permissions.
        /// </summary>
        public virtual DbSet<UserPermissionSetting> UserPermissionSettings { get; set; }

        /// <summary>
        /// OrganizationUnits.
        /// </summary>
        public virtual DbSet<OrganizationUnit> OrganizationUnits { get; set; }

        /// <summary>
        /// UserOrganizationUnits.
        /// </summary>
        public virtual DbSet<UserOrganizationUnit> UserOrganizationUnits { get; set; }

        /// <summary>
        /// OrganizationUnitRoles.
        /// </summary>
        public virtual DbSet<OrganizationUnitRole> OrganizationUnitRoles { get; set; }

        /// <summary>
        /// Tenants
        /// </summary>
        public virtual DbSet<Tenant> Tenants { get; set; }

        /// <summary>
        /// Editions.
        /// </summary>
        public virtual DbSet<Edition> Editions { get; set; }

        /// <summary>
        /// Settings.
        /// </summary>
        public virtual DbSet<Setting> Settings { get; set; }

        public AuthCenterDbContext(DbContextOptions<AuthCenterDbContext> options) 
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b => 
            {
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                b.HasOne(p => p.DelterUser)
                .WithMany()
                .HasForeignKey(p => p.DeleterUserId);

                b.HasOne(p => p.CreatorUser)
                .WithMany()
                .HasForeignKey(p => p.CreatorUserId);

                b.HasOne(p => p.LastModifierUser)
                .WithMany()
                .HasForeignKey(p => p.LastModifierUserId);              
            });

            modelBuilder.Entity<Role>(b => 
            {
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
            });

            modelBuilder.Entity<OrganizationUnit>(b => 
            {
                b.HasIndex(e => new { e.TenantId, e.Code }).IsUnique(false);
            });

            modelBuilder.Entity<PermissionSetting>(b => 
            {
                b.HasIndex(e => new { e.TenantId, e.Name });
            });

            modelBuilder.Entity<RoleClaim>(b =>
            {
                b.HasIndex(e => new { e.RoleId });
                b.HasIndex(e => new { e.TenantId, e.ClaimType });
            });

            modelBuilder.Entity<Role>(b => 
            {
                b.HasIndex(e => new { e.TenantId, e.NormalizedName });
            });

            modelBuilder.Entity<Setting>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Name, e.UserId }).IsUnique().HasFilter(null);
            });

            modelBuilder.Entity<UserClaim>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.ClaimType });
            });

            modelBuilder.Entity<UserOrganizationUnit>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.OrganizationUnitId });
            });

            modelBuilder.Entity<OrganizationUnitRole>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.RoleId });
                b.HasIndex(e => new { e.TenantId, e.OrganizationUnitId });
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.RoleId });
            });

            modelBuilder.Entity<User>(b => 
            {
                b.HasIndex(e => new { e.TenantId, e.NormalizedUserName });
            });
        }
    }
}
