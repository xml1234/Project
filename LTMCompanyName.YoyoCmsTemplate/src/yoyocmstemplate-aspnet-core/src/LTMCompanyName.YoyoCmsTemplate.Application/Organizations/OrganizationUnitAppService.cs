using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations
{
    [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits)]
    public class OrganizationUnitAppService : YoyoCmsTemplateAppServiceBase, IOrganizationUnitAppService
    {
        #region OrganizationUnit private

        private async Task<OrganizationUnitListDto> CreateOrganizationUnit(OrganizationUnit organizationUnit)
        {
            var dto = ObjectMapper.Map<OrganizationUnitListDto>(organizationUnit);
            dto.MemberCount =
                await _userOrganizationUnitRepository.CountAsync(uou => uou.OrganizationUnitId == organizationUnit.Id);
            return dto;
        }

        #endregion

        #region 初始化

        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;

        public OrganizationUnitAppService(
            OrganizationUnitManager organizationUnitManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
        }

        #endregion


        #region OrganizationUnit public

        public async Task<ListResultDto<OrganizationUnitListDto>> GetAllOrganizationUnitList()
        {
            var query =
                from ou in _organizationUnitRepository.GetAll()
                join uou in _userOrganizationUnitRepository.GetAll() on ou.Id equals uou.OrganizationUnitId into g
                select new { ou, memberCount = g.Count() };

            var items = await query.ToListAsync();


            return new ListResultDto<OrganizationUnitListDto>(
                items.Select(item =>
                {
                    var dto = ObjectMapper.Map<OrganizationUnitListDto>(item.ou);
                    dto.MemberCount = item.memberCount;
                    return dto;
                }).ToList());
        }


        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitListDto> Create(CreateOrganizationUnitInput input)
        {
            var organizationUnit = new OrganizationUnit(AbpSession.TenantId, input.DisplayName, input.ParentId);
            await _organizationUnitManager.CreateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnitListDto>(organizationUnit);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task Delete(EntityDto<long> input)
        {
            await _organizationUnitManager.DeleteAsync(input.Id);
        }


        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitListDto> Update(UpdateOrganizationUnitInput input)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(input.Id);

            if (organizationUnit == null)
                throw new UserFriendlyException("指定用户不存在");

            organizationUnit.DisplayName = input.DisplayName;

            await _organizationUnitManager.UpdateAsync(organizationUnit);

            return await CreateOrganizationUnit(organizationUnit);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitListDto> Move(MoveOrganizationUnitInput input)
        {
            await _organizationUnitManager.MoveAsync(input.Id, input.NewParentId);

            return await CreateOrganizationUnit(
                await _organizationUnitRepository.GetAsync(input.Id)
            );
        }

        #endregion


        #region Organization User public

        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageUsers)]
        public async Task<PagedResultDto<OrganizationUnitUserListDto>> GetPagedOrganizationUnitUsers(
            GetOrganizationUnitUsersInput input)
        {
            try
            {
                var query = from uou in _userOrganizationUnitRepository.GetAll()
                            join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                            join user in UserManager.Users.WhereIf(!input.FilterText.IsNullOrWhiteSpace(), u => u.UserName.Contains(input.FilterText)) on uou.UserId equals user.Id
                            where uou.OrganizationUnitId == input.Id
                            select new { uou, user };

                var totalCount = await query.CountAsync();
                var items = await query.OrderBy(o => input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<OrganizationUnitUserListDto>(
                    totalCount,
                    items.Select(item =>
                    {
                        var dto = ObjectMapper.Map<OrganizationUnitUserListDto>(item.user);
                        dto.AddedTime = item.uou.CreationTime;
                        return dto;
                    }).ToList());
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageUsers)]
        public async Task AddUsers(UsersToOrganizationUnitInput input)
        {
            foreach (var uids in input.UserIds)
                await UserManager.AddToOrganizationUnitAsync(uids, input.OrganizationUnitId);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageUsers)]
        public async Task RemoveUser(UserToOrganizationUnitInput input)
        {
            await UserManager.RemoveFromOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }

        public async Task<bool> IsInOrganizationUnit(UserToOrganizationUnitInput input)
        {
            return await UserManager.IsInOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageUsers)]
        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input)
        {
            var userIdsInOrganizationUnit = _userOrganizationUnitRepository.GetAll()
                .Where(uou => uou.OrganizationUnitId == input.OrganizationUnitId)
                .Select(uou => uou.UserId);

            var query = UserManager.Users
                .Where(user => !userIdsInOrganizationUnit.Contains(user.Id))
                .WhereIf(
                    !input.FilterText.IsNullOrWhiteSpace(),
                    user =>
                        user.UserName.Contains(input.FilterText) ||
                        user.EmailAddress.Contains(input.FilterText)
                );

            var userCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<NameValueDto>(
                userCount,
                users.Select(user =>
                    new NameValueDto(
                        $"{user.UserName} ({user.EmailAddress})",
                        user.Id.ToString()
                    )
                ).ToList()
            );
        }

        /// <summary>
        ///     批量从组织中移除用户
        /// </summary>
        /// <param name="userIds">用户Id列表</param>
        /// <param name="organizationUnitId">组织机构Id</param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageUsers)]
        public async Task BatchRemoveUserFromOrganizationUnit(List<long> userIds, long organizationUnitId)
        {
            await _userOrganizationUnitRepository.DeleteAsync(ou =>
                userIds.Contains(ou.UserId) && ou.OrganizationUnitId == organizationUnitId);
        }

        #endregion
    }
}