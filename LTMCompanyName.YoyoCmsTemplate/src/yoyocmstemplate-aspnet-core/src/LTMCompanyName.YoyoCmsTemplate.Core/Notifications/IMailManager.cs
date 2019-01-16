using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications
{
    public interface IMailManager
    {
        Task SendMessage(string toMailAddress, string title, string body);
    }
}
