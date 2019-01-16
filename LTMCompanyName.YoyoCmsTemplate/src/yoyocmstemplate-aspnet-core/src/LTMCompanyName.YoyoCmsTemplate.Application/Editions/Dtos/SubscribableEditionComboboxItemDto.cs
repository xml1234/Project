using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Editions.Dtos
{
    public class SubscribableEditionComboboxItemDto : ComboboxItemDtoT<int>
    {
        public bool? IsFree { get; set; }

        public SubscribableEditionComboboxItemDto(int value, string displayText, bool? isFree) 
            : base(value, displayText)
        {
            IsFree = isFree;
        }
    }
}
