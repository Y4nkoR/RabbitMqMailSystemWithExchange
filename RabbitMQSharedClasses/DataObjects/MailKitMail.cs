using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSharedClasses.DataObjects
{
    public class MailKitMail : Mail
    {
        public MailKitMail(string sender, List<string> recipients, string subject, string body) : base(sender, recipients, subject, body, MailType.MailKit)
        {
        }

        public override void Send()
        {
            // Implement here...
        }
    }
}
