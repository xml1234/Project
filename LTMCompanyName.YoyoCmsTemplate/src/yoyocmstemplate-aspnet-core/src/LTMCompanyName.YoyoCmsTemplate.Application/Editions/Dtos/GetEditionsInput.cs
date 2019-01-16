using Abp.Runtime.Validation;
using LTMCompanyName.YoyoCmsTemplate.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Editions.Dtos
{
    public class GetEditionsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
                Sorting = "CreationTime Desc";
        }
    }
}
