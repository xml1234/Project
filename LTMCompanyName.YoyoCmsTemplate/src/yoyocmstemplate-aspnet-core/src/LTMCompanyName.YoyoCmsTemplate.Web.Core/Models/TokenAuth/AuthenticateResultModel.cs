namespace LTMCompanyName.YoyoCmsTemplate.Models.TokenAuth
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }
        /// <summary>
        /// ��Ҫ��������
        /// </summary>
        public bool ShouldResetPassword { get; set; }

        public string PasswordResetCode { get; set; }


        public string ReturnUrl { get; set; }





    }
}
