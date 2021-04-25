namespace api.Helpers
{
    public static class EmailHelper
    {
        public static bool IsValidEmail(string email)
        {
            try {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch {
                return false;
            }
        }
    }
}