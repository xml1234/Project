namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos
{
    /// <summary>
    ///     ��ȡ����ͼƬ�����DTO
    /// </summary>
    public class GetProfilePictureOutputDto
    {
        public GetProfilePictureOutputDto(string profilePicture)
        {
            ProfilePicture = profilePicture;
        }

        public string ProfilePicture { get; set; }
    }
}