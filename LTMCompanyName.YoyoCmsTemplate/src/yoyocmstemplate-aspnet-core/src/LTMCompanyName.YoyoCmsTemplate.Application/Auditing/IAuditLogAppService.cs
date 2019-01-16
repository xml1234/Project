using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Auditing
{
    /// <summary>
    ///     审计日志服务
    /// </summary>
    public interface IAuditLogAppService : IApplicationService
    {
        Task<PagedResultDto<AuditLogListDto>> GetPagedAuditLogs(GetAuditLogsInput input);
    }
}