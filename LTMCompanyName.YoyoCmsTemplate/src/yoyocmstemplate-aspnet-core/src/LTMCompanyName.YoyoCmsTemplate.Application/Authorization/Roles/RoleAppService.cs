using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Roles
{
    /// <summary>
    ///     角色服务
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Administration_Roles)]
    public class RoleAppService : YoyoCmsTemplateAppServiceBase, IRoleAppService
    {
        #region 初始化

        private readonly RoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;

        public RoleAppService(RoleManager roleManager, IPermissionManager permissionManager)
        {
            _roleManager = roleManager;
            _permissionManager = permissionManager;
        }

        #endregion

        #region  公有方法

        /// <summary>
        ///     服务于前端RoleComboxComponent 组件信息，
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Roles)]
        public async Task<ListResultDto<RoleListDto>> GetAll(GetRolesInput input)
        {
            var roles = await _roleManager
                .Roles
                .WhereIf(
                    !input.Permission.IsNullOrWhiteSpace(),
                    r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
                )
                .ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles)]
        public async Task<PagedResultDto<RoleListDto>> GetPaged(GetRolePagedInput input)
        {
            var query = _roleManager
                .Roles
                .WhereIf(
                    !input.FilterText.IsNullOrWhiteSpace(),
                    r =>
                        r.Name.Contains(input.FilterText) ||
                        r.DisplayName.Contains(input.FilterText)
                )
                .WhereIf(
                    input.PermissionNames != null && input.PermissionNames.Count > 0,
                    r => r.Permissions.Any(rp => input.PermissionNames.Contains(rp.Name) && rp.IsGranted)
                );


            // // 判断权限符合当前权限列表的角色有哪些

            // if (input.PermissionNames!=null&&input.PermissionNames.Count>0)
            // {
            //     foreach (var permissionName in input.PermissionNames)
            //     {
            //query=         query.Where(a => a.Permissions.Any(p => p.Name == permissionName && p.IsGranted == true));
            //         //permissionName

            //     }
            // }


            var count = await query.CountAsync();

            var roles = await query
                .PageBy(input)
                .ToListAsync();


            return new PagedResultDto<RoleListDto>
            {
                TotalCount = count,
                Items = ObjectMapper.Map<List<RoleListDto>>(roles)
            };
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Create,
            PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task<GetRoleForEditOutput> GetForEdit(NullableIdDto input)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = new Permission[0];
            RoleEditDto roleEditDto;

            if (input.Id.HasValue) //Editing existing role?
            {
                var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
                grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
                roleEditDto = ObjectMapper.Map<RoleEditDto>(role);
            }
            else
            {
                roleEditDto = new RoleEditDto();
            }

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Create,
            PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateRoleInput input)
        {
            if (input.Role.Id.HasValue)
                await UpdateRoleAsync(input);
            else
                await CreateRoleAsync(input);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Delete)]
        public async Task Delete(EntityDto input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task UpdatePermissions(UpdateRolePermissionsInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.RoleId);
            var grantedPermissions = _permissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissionNames.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }

        /// <summary>
        ///     批量删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task BatchDeleteAsync(List<int> ids)
        {
            foreach (var id in ids)
            {
                var role = await _roleManager.GetRoleByIdAsync(id);
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                foreach (var user in users) CheckErrors(await UserManager.RemoveFromRoleAsync(user, role.Name));
                CheckErrors(await _roleManager.DeleteAsync(role));
            }
        }

        #endregion

        #region 私有方法

        private async Task UpdateRoleAsync(CreateOrUpdateRoleInput input)
        {
            Debug.Assert(input.Role.Id != null, "input.Role.Id should be set.");

            var role = await _roleManager.GetRoleByIdAsync(input.Role.Id.Value);
            role.DisplayName = input.Role.DisplayName;
            role.IsDefault = input.Role.IsDefault;

            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
        }

        private async Task CreateRoleAsync(CreateOrUpdateRoleInput input)
        {
            var role = new Role(AbpSession.TenantId, input.Role.DisplayName) {IsDefault = input.Role.IsDefault};
            CheckErrors(await _roleManager.CreateAsync(role));
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the role.
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
        }

        private async Task UpdateGrantedPermissionsAsync(Role role, List<string> grantedPermissionNames)
        {
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(grantedPermissionNames);
            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }

        #endregion
    }
}