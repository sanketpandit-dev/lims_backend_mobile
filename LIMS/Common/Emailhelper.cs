using System.Net;
using System.Net.Mail;

namespace LIMS.Common
{
    public class Emailhelper
    {
        public static void SendEmail(string to, string subject, string body)
        {
            try
            {

                var fromAddress = new MailAddress("libraryms.alphonsol@gmail.com", "LIMS");
                var toAddress = new MailAddress(to);
                const string fromPassword = "zqft jirb voky txrr";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}

