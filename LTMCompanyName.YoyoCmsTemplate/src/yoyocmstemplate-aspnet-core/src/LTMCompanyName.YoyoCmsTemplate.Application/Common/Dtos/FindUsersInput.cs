using LTMCompanyName.YoyoCmsTemplate.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Common.Dtos
{
    public class CommonLookupFindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}