namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos
{
    public class EmailSettingsEditDto
    {
        //û�н�����֤����Ϊ���ܲ���ʹ���ʼ�ϵͳ��

        /// <summary>
        /// Ĭ�Ϸ����������ַ
        /// </summary>
        public string DefaultFromAddress { get; set; }

        /// <summary>
        /// ������ʾ����
        /// </summary>
        public string DefaultFromDisplayName { get; set; }

        /// <summary>
        /// ����������SMTP������Host
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// ����������SMTP�������˿�
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// ������У������
        /// </summary>
        public string SmtpUserName { get; set; }

        /// <summary>
        /// ������У������
        /// </summary>
        public string SmtpPassword { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string SmtpDomain { get; set; }

        /// <summary>
        /// ʹ��ssl
        /// </summary>
        public bool SmtpEnableSsl { get; set; }

        /// <summary>
        /// ʹ��Ĭ��ƾ��
        /// </summary>
        public bool SmtpUseDefaultCredentials { get; set; }
    }
}