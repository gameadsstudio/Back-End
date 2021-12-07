using System;
using System.Net.Mail;
using System.Net;

namespace api.Business.Mail
{
    public class MailBusinessLogic: IMailBusinessLogic
    {
        public MailBusinessLogic()
        {
        }

        public void send(string to, string subject, string content)
        {
            SmtpClient client = new SmtpClient(
                Environment.GetEnvironmentVariable("GAS_MAIL_HOST"),
                Int32.Parse(
                    Environment.GetEnvironmentVariable("GAS_MAIL_PORT")
                )
            )
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    Environment.GetEnvironmentVariable("GAS_MAIL_USERNAME"),
                    Environment.GetEnvironmentVariable("GAS_MAIL_PASSWORD")
                ),
                EnableSsl = bool.Parse(
                    Environment.GetEnvironmentVariable("GAS_MAIL_SSL")
                )
            };

            client.Send(new MailMessage(
                Environment.GetEnvironmentVariable(
                    "GAS_MAIL_ADR_NO_REPLY"
                ),
                to,
                subject,
                content
            ));
        }
    }
}
