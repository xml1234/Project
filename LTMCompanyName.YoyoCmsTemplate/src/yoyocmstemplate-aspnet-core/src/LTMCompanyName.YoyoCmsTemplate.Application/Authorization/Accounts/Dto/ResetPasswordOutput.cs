using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto
{
    public class ResetPasswordOutput
    {
        public bool CanLogin { get; set; }

        public string UserName { get; set; }
    }

    public class ResetPasswordInput
    {
        [Range(1, long.MaxValue)] public long UserId { get; set; }

        [Required] public string ResetCode { get; set; }

        [Required] [DisableAuditing] public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public string SingleSignIn { get; set; }
    }
}