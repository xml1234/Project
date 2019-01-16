using System;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Localization;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.Timing;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.Timing;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile
{
    public class ProfileAppService : YoyoCmsTemplateAppServiceBase, IProfileAppService
    {
        private const int MaxProfilPictureBytes = 1048576; //1MB
        private readonly IAppFolder _appFolders;
        private readonly ICacheManager _cacheManager;
        private readonly IDataFileObjectManager _dataFileObjectManager;
        private readonly ITimeZoneService _timeZoneService;


        public ProfileAppService(
            IAppFolder appFolders,
            ITimeZoneService timezoneService,
            ICacheManager cacheManager, IDataFileObjectManager dataFileObjectManager)
        {
            _appFolders = appFolders;
            _timeZoneService = timezoneService;
            _cacheManager = cacheManager;
            _dataFileObjectManager = dataFileObjectManager;
        }


        [DisableAuditing]
        public async Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit()
        {
            var user = await GetCurrentUserAsync();
            var userProfileEditDto = ObjectMapper.Map<CurrentUserProfileEditDto>(user);


            //if (Clock.SupportsMultipleTimezone)
            //{
            //    userProfileEditDto.Timezone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);

            //    var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
            //    if (userProfileEditDto.Timezone == defaultTimeZoneId)
            //    {
            //        userProfileEditDto.Timezone = string.Empty;
            //    }
            //}

            return userProfileEditDto;
        }

        public async Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input)
        {
            var user = await GetCurrentUserAsync();

            if (user.PhoneNumber != input.PhoneNumber)
                input.IsPhoneNumberConfirmed = false;
            else if (user.IsPhoneNumberConfirmed) input.IsPhoneNumberConfirmed = true;

            ObjectMapper.Map(input, user);
            CheckErrors(await UserManager.UpdateAsync(user));

            if (Clock.SupportsMultipleTimezone)
            {
                var defaultValue =
                    await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(),
                    TimingSettingNames.TimeZone, defaultValue);

                //if (input.Timezone.IsNullOrEmpty())
                //{
                //    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                //    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, defaultValue);
                //}
                //else
                //{
                //    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, input.Timezone);
                //}
            }
        }


        public async Task ChangePassword(ChangePasswordInput input)
        {
            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await GetCurrentUserAsync();
            CheckErrors(await UserManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword));
        }


        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        public async Task<GetProfilePictureOutputDto> GetProfilePictureByIdAsync(Guid profilePictureId)
        {
            return await GetProfilePictureByIdInternal(profilePictureId);
        }

        /// <summary>
        ///     ɾ���û�����ͷ��
        /// </summary>
        /// <param name="profilePictureId">ͷ��Id</param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Users_DeleteProfilePicture)]
        public async Task DeleteProfilePictureById(Guid profilePictureId)
        {
            await _dataFileObjectManager.DeleteAsync(profilePictureId);
        }


        #region �ڲ�����

        /// <summary>
        ///     ͨ��id��ȡ����ͷ�񣬿���Ϊ��
        /// </summary>
        /// <param name="profilePictureId"></param>
        /// <returns></returns>
        private async Task<byte[]> GetProfilePictureByIdOrNull(Guid profilePictureId)
        {
            var file = await _dataFileObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null) return null;

            return file.Bytes;
        }

        /// <summary>
        ///     �ڲ���ȡͷ��
        /// </summary>
        /// <param name="profilePictureId"></param>
        /// <returns></returns>
        private async Task<GetProfilePictureOutputDto> GetProfilePictureByIdInternal(Guid profilePictureId)
        {
            var bytes = await GetProfilePictureByIdOrNull(profilePictureId);
            if (bytes == null) return new GetProfilePictureOutputDto(string.Empty);

            return new GetProfilePictureOutputDto(Convert.ToBase64String(bytes));
        }

        #endregion
    }
}