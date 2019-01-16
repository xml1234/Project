using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Localization;
using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Sessions.Dto;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Languages.Dtos.CustomMapper
{
    /// <summary>
    ///     配置Language的AutoMapper
    /// </summary>
    internal static class CustomerLanguageMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();
            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<User, UserMiniDto>();
            // Language
            configuration.CreateMap<ApplicationLanguage, LanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, LanguageListDto>();
            configuration.CreateMap<ApplicationLanguage, LanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();
        }
    }
}