using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSharedClasses.DataObjects
{
    public class SmtpMail : Mail
    {
        public SmtpMail(string sender, List<string> recipients, string subject, string body) : base(sender, recipients, subject, body, MailType.Smtp)
        {
        }

        public override void Send()
        {
            var smtpClient = new SmtpClient("smtp.ethereal.email")
            {
                Port = 587,
                Credentials = new NetworkCredential("andre83@ethereal.email", "jrUxDJvfkkSkH8gDck"),     // In practice, it should not be stored here.
                EnableSsl = true,
            };

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Sender);

            foreach (var recipient in Recipients)
            {
                mailMessage.To.Add(new MailAddress(recipient));
            }

            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;

            smtpClient.Send(mailMessage);
        }
    }
}
