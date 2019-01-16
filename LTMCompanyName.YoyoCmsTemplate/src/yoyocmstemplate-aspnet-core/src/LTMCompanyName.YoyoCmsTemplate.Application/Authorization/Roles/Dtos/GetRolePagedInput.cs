using System.Collections.Generic;
using Abp.Runtime.Validation;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Roles.Dtos
{
    public class GetRolePagedInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        /// <summary>
        ///     Ȩ���б�����
        /// </summary>
        public List<string> PermissionNames { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting)) Sorting = "Id";
        }
    }
}