using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQSharedClasses;
using RabbitMQSharedClasses.Interfaces;
using RabbitMQSharedClasses.DataObjects;
using System.Net.Mail;
using System.Net;
using System;
using System.Threading.Channels;

namespace MailReceiver
{
    class Program
    {
        private static string ExchangeName { get; set; }
        private static IConnection Connection { get; set; }

        static void Main(string[] args)
        {
            IRabbitMQService rabbitMQService = new RabbitMQService();
            Connection = rabbitMQService.GetConnection();
            ExchangeName = rabbitMQService.GetMailExchangeName();

            ReceiveSmtpMail();
            ReceiveMailKitMail();

            Console.WriteLine("Waiting for mails...");
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        private static void ReceiveSmtpMail()
        {
            var channel = Connection.CreateModel();
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            var queue = channel.QueueDeclare(queue: "SmtpMailQueue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            channel.QueueBind(queue: queue.QueueName,
                    exchange: ExchangeName,
                    routingKey: MailType.Smtp.ToString());

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mail = JsonSerializer.Deserialize<SmtpMail>(Encoding.UTF8.GetString(body));

                if (mail == null)
                {
                    throw new ArgumentException("Received data was not a Smtp mail!");
                }

                PrintMailToConsole(nameof(ReceiveSmtpMail), mail);
                mail.Send();
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: queue.QueueName,
                    autoAck: false,
                    consumer: consumer);
        }

        private static void ReceiveMailKitMail()
        {
            var channel = Connection.CreateModel();
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            var queue = channel.QueueDeclare(queue: "MailKitMailQueue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            channel.QueueBind(queue: queue.QueueName,
                    exchange: ExchangeName,
                    routingKey: MailType.MailKit.ToString());

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mail = JsonSerializer.Deserialize<MailKitMail>(Encoding.UTF8.GetString(body));

                if (mail == null)
                {
                    throw new ArgumentException("Received data was not an MailKit mail!");
                }

                PrintMailToConsole(nameof(ReceiveMailKitMail), mail);
                mail.Send();
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: queue.QueueName,
                    autoAck: false,
                    consumer: consumer);
        }

        private static void PrintMailToConsole(string callSource, Mail mail)
        {
            var recipients = String.Join(", ", mail.Recipients);
            Console.WriteLine($"({callSource})[{mail.MailType}] From: {mail.Sender}, To: {recipients}, Subject: {mail.Subject}, Body: {mail.Body}");
        }
    }
}