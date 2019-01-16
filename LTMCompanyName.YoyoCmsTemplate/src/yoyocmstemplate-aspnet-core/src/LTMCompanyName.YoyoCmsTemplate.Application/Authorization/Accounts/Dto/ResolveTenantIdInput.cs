using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto
{
    public class ResolveTenantIdInput
    {
        // 包含tenantId={value}字符串的加密文本
        public string c { get; set; }
    }
}
