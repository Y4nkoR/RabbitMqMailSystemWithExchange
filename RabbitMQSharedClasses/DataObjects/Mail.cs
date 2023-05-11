using System.Net.Mail;

namespace RabbitMQSharedClasses.DataObjects
{
    public enum MailType
    {
        Smtp,
        MailKit
    }

    public abstract class Mail
    {
        public string Sender { get; private set; }
        public List<string> Recipients { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public MailType MailType { get; private set; }

        public Mail(string sender, List<string> recipients, string subject, string body, MailType mailType)
        {
            Sender = sender;
            Recipients = recipients;
            Subject = subject;
            Body = body;
            MailType = mailType;
        }

        public abstract void Send();
    }
}
