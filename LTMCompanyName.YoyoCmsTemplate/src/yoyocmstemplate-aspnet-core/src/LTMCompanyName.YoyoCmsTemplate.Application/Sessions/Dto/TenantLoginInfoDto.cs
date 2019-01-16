using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;

namespace LTMCompanyName.YoyoCmsTemplate.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }

        public Guid? LogoId { get; set; }


        public DateTime CreationTime { get; set; }


        public string CreationTimeString { get; set; }
    }
}