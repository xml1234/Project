using Abp.Authorization;
using AutoMapper;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos.CustomMapper
{
    internal static class CustomerPermissionsMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Permission, TreePermissionDto>();
        }
    }
}