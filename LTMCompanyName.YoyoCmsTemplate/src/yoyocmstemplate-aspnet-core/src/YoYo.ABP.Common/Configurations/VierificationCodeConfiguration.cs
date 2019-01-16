using YoYo.ABP.Common.VierificationCode;

namespace YoYo.ABP.Common.Configurations
{
    public class VierificationCodeConfiguration
    {
        public bool IsEnabled { get; set; }

        public int Length { get; set; } = 4;

        public ValidateCodeType Type { get; set; } = ValidateCodeType.Number;
    }
}
