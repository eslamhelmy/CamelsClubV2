using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CamelsClub.Services.Helpers
{
    public class EmailHelper
    {

        public static bool SendMailTest2(string ToEmail, string EmailSubject, string EmailBody)
        {
            try
            {
                string senderMail = "eslamhelmy523@gmail.com";
                string senderPassword = "eslam@123456789";
                SmtpClient client = new SmtpClient("smtp.gmail.com",587);
                var host = client.Host;
                if (host == "smtp.gmail.com")
                    client.EnableSsl = true;
                else
                    client.EnableSsl = false;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderMail, senderPassword);
                MailMessage mailMessage = new MailMessage(senderMail, "eslamhelmy523@gmail.com", EmailSubject, EmailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
       
    }
}
