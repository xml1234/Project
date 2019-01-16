using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos
{
    [AutoMapFrom(typeof(User))]
    public class OrganizationUnitUserListDto : EntityDto<long>
    {
        public string UserName { get; set; }

        public DateTime AddedTime { get; set; }
    }
}