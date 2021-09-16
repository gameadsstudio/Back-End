using System.Net.Mail;

namespace api.Business.Mail
{
    public interface IMailBusinessLogic
    {
        void send(MailMessage mailMessage);
    }
}
