using System.Net.Mail;

namespace api.Helpers
{
    public static class EmailHelper
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}