using Abp.Configuration;
using Abp.Extensions;
using Abp.MailKit;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications
{
    public class YoYoMailKitSmtpBuilder : DefaultMailKitSmtpBuilder
    {
        public YoYoMailKitSmtpBuilder(
            ISmtpEmailSenderConfiguration smtpEmailSenderConfiguration,
            IAbpMailKitConfiguration abpMailKitConfiguration
            )
            : base(smtpEmailSenderConfiguration, abpMailKitConfiguration)
        {

        }


        protected override void ConfigureClient(MailKit.Net.Smtp.SmtpClient client)
        {
            client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            base.ConfigureClient(client);
        }

    }
}
