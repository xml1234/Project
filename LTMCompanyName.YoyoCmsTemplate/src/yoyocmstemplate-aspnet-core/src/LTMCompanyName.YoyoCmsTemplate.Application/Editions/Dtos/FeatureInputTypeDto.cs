using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Editions.Dtos
{
    //Mapped in CustomDtoMapper
    public class FeatureInputTypeDto
    {
        public string Name { get; set; }

        public IDictionary<string, object> Attributes { get; set; }

        public IValueValidator Validator { get; set; }

        public LocalizableComboboxItemSourceDto ItemSource { get; set; }
    }
}
