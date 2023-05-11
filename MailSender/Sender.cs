using RabbitMQ.Client;
using RabbitMQSharedClasses;
using RabbitMQSharedClasses.DataObjects;
using RabbitMQSharedClasses.Interfaces;
using System.Text;
using System.Text.Json;

namespace MailSender
{
    public class Sender
    {
        private string ExchangeName { get; set; }
        private IModel Channel { get; set; }

        public Sender() 
        {
            IRabbitMQService rabbitService = new RabbitMQService();
            var connection = rabbitService.GetConnection();
            ExchangeName = rabbitService.GetMailExchangeName();

            Channel = connection.CreateModel();
            Channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
        }

        public void Send(Mail mail)
        {
            var serializedMail = JsonSerializer.Serialize(mail);

            Channel.BasicPublish(exchange: ExchangeName,
                routingKey: mail.MailType.ToString(),
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(serializedMail));
        }
    }
}