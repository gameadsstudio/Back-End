using System.Net.Mail;

namespace api.Business.Mail
{
    public interface IMailBusinessLogic
    {
        void send(string to, string subject, string content);
    }
}
