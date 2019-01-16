using System.Threading.Tasks;
using Abp;
using Abp.Notifications;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications
{
    public class NotificationManager : YoyoCmsTemplateDomainServiceBase, INotificationManager
    {

        private readonly INotificationPublisher _notificationPublisher;

        public NotificationManager(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task WelcomeToApplicationAsync(User user)
        {

            await _notificationPublisher.PublishAsync(
                YoyoCmsTemplateConsts.NotificationConstNames.WelcomeToCms,
                new MessageNotificationData(L("WelcomeToApplication")),severity:NotificationSeverity.Success,userIds:new []{user.ToUserIdentifier()}
                
                
                
                );

        }

        public async Task SendMessageAsync(UserIdentifier user, string messager, NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(
                YoyoCmsTemplateConsts.NotificationConstNames.SendMessageAsync,
                new MessageNotificationData(messager),severity:severity,userIds:new []{user});
        }
    }
}