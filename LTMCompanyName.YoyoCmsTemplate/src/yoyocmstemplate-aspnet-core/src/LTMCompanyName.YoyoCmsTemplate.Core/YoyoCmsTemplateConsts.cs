namespace LTMCompanyName.YoyoCmsTemplate
{

    /// <summary>
    /// YoyoCmsTemplate的常量字段
    /// </summary>
    public class YoyoCmsTemplateConsts
    {
        /// <summary>
        /// 语言文件的名称
        /// </summary>
        public const string LocalizationSourceName = "YoyoCmsTemplate";

        /// <summary>
        /// 默认语言
        /// </summary>
        public const string DefaultLanguage = "zh-Hans";

        /// <summary>
        /// 数据库链接字符默认名称
        /// </summary>
        public const string ConnectionStringName = "Default";

        /// <summary>
        /// 多租户启用
        /// </summary>
        public const bool MultiTenancyEnabled = true;

        /// <summary>
        /// 默认一页显示多少条数据
        /// </summary>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// 一次性最多可以分多少页
        /// </summary>
        public const int MaxPageSize = 1000;
        /// <summary>
        /// 邮件地址最大长度
        /// </summary>
        public const int MaxEmailAddressLength = 250;

        /// <summary>
        /// 名字最大长度
        /// </summary>
        public const int MaxNameLength = 50;


        public const int MaxAddressLength = 250;

        public const int PaymentCacheDurationInMinutes = 30;

        /// <summary>
        /// 通知系统的常量管理
        /// </summary>
        public static class NotificationConstNames
        {
            /// <summary>
            /// 欢迎语
            /// </summary>
            public const string WelcomeToCms = "App.WelcomeToCms";
            /// <summary>
            /// 发送消息信息
            /// </summary>
            public const string SendMessageAsync = "App.SendMessageAsync";
        }

    }
}
