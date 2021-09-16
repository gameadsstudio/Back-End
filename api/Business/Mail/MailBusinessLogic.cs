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

        public void send(
            MailMessage mailMessage
        )
        {
            SmtpClient client = new SmtpClient(
                Environment.GetEnvironmentVariable(
                    "GAS_MAIL_HOST"
                ) ?? "smtp.example.com",
                Int32.Parse(
                    Environment.GetEnvironmentVariable(
                        "GAS_MAIL_PORT"
                    ) ?? "587"
                )
            )
            {
                Credentials = new NetworkCredential(
                    Environment.GetEnvironmentVariable(
                        "GAS_MAIL_USERNAME"
                    ) ?? "smtp_username",
                    Environment.GetEnvironmentVariable(
                        "GAS_MAIL_PASSWORD"
                    ) ?? "smtp_password"
                ),
                EnableSsl = bool.Parse(
                    Environment.GetEnvironmentVariable("GAS_MAIL_SSL") ?? "true"
                )
            };

            client.Send(mailMessage);
        }
    }
}
