using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Debugging
{
    public static class DebugHelper
    {
        /// <summary>
        /// 是否为Debug模式
        /// </summary>
        public const bool IsDebug =
#if DEBUG
            true
#endif
#if !DEBUG
            false
#endif
        ;

    }
}
