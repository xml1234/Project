using Abp.Configuration;


namespace LTMCompanyName.YoyoCmsTemplate.Timing.Dtos
{
    public class GetTimezoneComboboxItemsInput
    {
        public SettingScopes DefaultTimezoneScope;

        public string SelectedTimezoneId { get; set; }
    }
}
