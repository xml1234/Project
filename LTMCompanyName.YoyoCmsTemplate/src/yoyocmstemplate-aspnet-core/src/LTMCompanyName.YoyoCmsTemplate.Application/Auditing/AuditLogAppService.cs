using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.NamespaceHelper;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;



namespace LTMCompanyName.YoyoCmsTemplate.Auditing
{
    [DisableAuditing]
    [AbpAuthorize(PermissionNames.Pages_AdminiStration_AuditLogs)]
    public class AuditLogAppService : YoyoCmsTemplateAppServiceBase, IAuditLogAppService
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly NamespaceHelperManager _namespaceHelperManager;
        private readonly IRepository<User, long> _userRepository;

        public AuditLogAppService(IRepository<AuditLog, long> auditLogRepository,
            IRepository<User, long> userRepository, NamespaceHelperManager namespaceHelperManager)
        {
            _auditLogRepository = auditLogRepository;
            _userRepository = userRepository;
            _namespaceHelperManager = namespaceHelperManager;
        }


        public async Task<PagedResultDto<AuditLogListDto>> GetPagedAuditLogs(GetAuditLogsInput input)
        {
            var query = CreateAuditLogAndUsersQuery(input);

            var resultCount = await query.CountAsync();
            var results = await query
                .AsNoTracking()
                .OrderBy(input.Sorting)// TODO: OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var auditLogListDtos = ConvertToAuditLogListDtos(results);

            return new PagedResultDto<AuditLogListDto>(resultCount, auditLogListDtos);
        }

        #region 审计日志私有服务

        /// <summary>
        ///     创建审计日志用户的查询服务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private IQueryable<AuditLogAndUser> CreateAuditLogAndUsersQuery(GetAuditLogsInput input)
        {
            IQueryable<AuditLogAndUser> query;
            if (input.StartDate.HasValue && input.EndDate.HasValue)
                query = from auditLog in _auditLogRepository.GetAll()
                        join user in _userRepository.GetAll() on auditLog.UserId equals user.Id into userJoin
                        from joinedUser in userJoin.DefaultIfEmpty()
                        where auditLog.ExecutionTime >= input.StartDate && auditLog.ExecutionTime <= input.EndDate
                        select new AuditLogAndUser { AuditLogInfo = auditLog, UserInfo = joinedUser };
            else
                query = from auditLog in _auditLogRepository.GetAll()
                        join user in _userRepository.GetAll() on auditLog.UserId equals user.Id into userJoin
                        from joinedUser in userJoin.DefaultIfEmpty()
                        select new AuditLogAndUser { AuditLogInfo = auditLog, UserInfo = joinedUser };

            query = query
                .WhereIf(!input.UserName.IsNullOrWhiteSpace(), item => item.UserInfo.UserName.Contains(input.UserName))
                .WhereIf(!input.ServiceName.IsNullOrWhiteSpace(),
                    item => item.AuditLogInfo.ServiceName.Contains(input.ServiceName))
                .WhereIf(!input.MethodName.IsNullOrWhiteSpace(),
                    item => item.AuditLogInfo.MethodName.Contains(input.MethodName))
                .WhereIf(!input.BrowserInfo.IsNullOrWhiteSpace(),
                    item => item.AuditLogInfo.BrowserInfo.Contains(input.BrowserInfo))
                .WhereIf(input.MinExecutionDuration.HasValue && input.MinExecutionDuration > 0,
                    item => item.AuditLogInfo.ExecutionDuration >= input.MinExecutionDuration.Value)
                .WhereIf(input.MaxExecutionDuration.HasValue && input.MaxExecutionDuration < int.MaxValue,
                    item => item.AuditLogInfo.ExecutionDuration <= input.MaxExecutionDuration.Value)
                .WhereIf(input.HasException == true,
                    item => item.AuditLogInfo.Exception != null && item.AuditLogInfo.Exception != "")
                .WhereIf(input.HasException == false,
                    item => item.AuditLogInfo.Exception == null || item.AuditLogInfo.Exception == "");
            return query;
        }

        private List<AuditLogListDto> ConvertToAuditLogListDtos(List<AuditLogAndUser> results)
        {
            return results.Select(
                result =>
                {
                    var auditLogListDto = ObjectMapper.Map<AuditLogListDto>(result.AuditLogInfo);
                    auditLogListDto.UserName = result.UserInfo == null ? null : result.UserInfo.UserName;
                    auditLogListDto.ServiceName = _namespaceHelperManager.SplitNameSpace(auditLogListDto.ServiceName);
                    return auditLogListDto;
                }).ToList();
        }

        #endregion
    }
}