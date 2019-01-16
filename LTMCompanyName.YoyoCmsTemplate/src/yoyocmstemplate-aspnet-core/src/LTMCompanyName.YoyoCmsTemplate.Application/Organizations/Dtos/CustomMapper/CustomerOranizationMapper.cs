using Abp.Organizations;
using AutoMapper;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos.CustomMapper
{
    internal static class CustomerOranizationMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //configuration.CreateMap<OrganizationUnit, OrganizationUnitTreeDto>();
            configuration.CreateMap<OrganizationUnit, OrganizationUnitListDto>();
        }
    }
}