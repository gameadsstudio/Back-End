using System.Net.Mail;

namespace api.Business.Mail
{
    interface IMailBusinessLogic
    {
        void send(MailMessage mailMessage);
    }
}
