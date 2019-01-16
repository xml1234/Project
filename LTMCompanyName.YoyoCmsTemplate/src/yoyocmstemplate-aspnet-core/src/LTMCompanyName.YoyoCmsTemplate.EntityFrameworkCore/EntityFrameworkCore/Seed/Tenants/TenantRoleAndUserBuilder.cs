using System.Linq;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Editions.Authorization;

namespace LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly YoyoCmsTemplateDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(YoyoCmsTemplateDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();

            //CreateMemberRoles();
        }

        private void CreateRolesAndUsers()
        {
            // Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters()
                .FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles
                    .Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin)
                    {
                        IsStatic = true,
                        IsDefault = false
                    }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(
                    new YoyoCmsTemplateAuthorizationProvider(false),
                    new EditionAuthorizationProvider(false)
                )
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    })
                );
                _context.SaveChanges();
            }

            // Admin user

            var adminUser = _context.Users.IgnoreQueryFilters()
                .FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "ltm@ddxc.org");
                adminUser.Password =
                    new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions()))
                        .HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;
                adminUser.NeedToChangeThePassword = false;
                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }
        }


        /// <summary>
        /// ������Ա��ɫ
        /// </summary>
        private void CreateMemberRoles()
        {
            #region һ����Ա

            {
                var lv1Role = _context.Roles.IgnoreQueryFilters()
              .FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Member_Lv1);
                if (lv1Role == null)
                {
                    lv1Role = _context.Roles
                        .Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin)
                        {
                            IsStatic = true,
                            IsDefault = true// Ĭ��ע����һ����Ա

                        }).Entity;
                    _context.SaveChanges();
                }

                // Grant all permissions to lv1 role

                var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                    .OfType<RolePermissionSetting>()
                    .Where(p => p.TenantId == _tenantId && p.RoleId == lv1Role.Id)
                    .Select(p => p.Name)
                    .ToList();

                var permissions = PermissionFinder
                    .GetAllPermissions(new YoyoCmsTemplateAuthorizationProvider(false))
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                                !grantedPermissions.Contains(p.Name))
                    .ToList();

                if (permissions.Any())
                {
                    _context.Permissions.AddRange(
                        permissions.Select(permission => new RolePermissionSetting
                        {
                            TenantId = _tenantId,
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = lv1Role.Id
                        })
                    );
                    _context.SaveChanges();
                }
            }

            #endregion


            #region ������Ա

            {
                var lv2Role = _context.Roles.IgnoreQueryFilters()
                .FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Member_Lv2);
                if (lv2Role == null)
                {
                    lv2Role = _context.Roles
                        .Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin)
                        {
                            IsStatic = true
                        }).Entity;
                    _context.SaveChanges();
                }

                // Grant all permissions to lv2 role

                var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                    .OfType<RolePermissionSetting>()
                    .Where(p => p.TenantId == _tenantId && p.RoleId == lv2Role.Id)
                    .Select(p => p.Name)
                    .ToList();

                var permissions = PermissionFinder
                    .GetAllPermissions(new YoyoCmsTemplateAuthorizationProvider(false))
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                                !grantedPermissions.Contains(p.Name))
                    .ToList();

                if (permissions.Any())
                {
                    _context.Permissions.AddRange(
                        permissions.Select(permission => new RolePermissionSetting
                        {
                            TenantId = _tenantId,
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = lv2Role.Id
                        })
                    );
                    _context.SaveChanges();
                }
            }

            #endregion
        }
    }
}