using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Mapper
{

    /// <summary>
    /// 配置 Tenant 的AutoMapper
    /// </summary>
    internal static class TenantMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

        }
    }
}
