using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos
{
    /// <summary>
    ///   组织单元列表Dto
    /// </summary>
    public class OrganizationUnitListDto : AuditedEntityDto<long>
    {
        public long? ParentId { get; set; }

        public string Code { get; set; }

        public string DisplayName { get; set; }

        public int MemberCount { get; set; }
    }
}