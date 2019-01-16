using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Editions.Dtos
{
    public class EditionWithFeaturesDto
    {
        public EditionWithFeaturesDto()
        {
            FeatureValues = new List<NameValueDto>();
        }

        public EditionSelectDto Edition { get; set; }

        public List<NameValueDto> FeatureValues { get; set; }
    }
}
